using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour {

    private Vector3 mousePosition;
    private Vector3 worldPosition;

    private Tilemap tilemap;
    private List<Vector3> placeablePositions;
    private Vector3Int cellPosition;
    private Vector3 temp;
    private Vector3Int localPlace;
    private Vector3 cellCenter;

    public Transform blockPrefab;

    void Start() {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        cellPosition = tilemap.WorldToCell(transform.position);
        transform.position = tilemap.GetCellCenterWorld(cellPosition);
        placeablePositions = new List<Vector3>();

        for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++) {
            for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++) {
                localPlace = new Vector3Int(n, p, (int)tilemap.transform.position.y);
                cellCenter = tilemap.GetCellCenterWorld(localPlace);
                if (tilemap.HasTile(localPlace)) {
                    placeablePositions.Add(cellCenter);
                }
            }
        }
    }

    private void Update() {
        cellPosition = tilemap.WorldToCell(getMousePosition());
        temp = tilemap.GetCellCenterWorld(cellPosition);

        if (Vector3.Distance(transform.position, temp) < 0.25f && placeablePositions.Contains(temp)) {
            transform.position = Vector3.MoveTowards(transform.position, temp, 3f * Time.deltaTime);

            if (Input.GetMouseButtonDown(0) && CanPlace() && Vector2.Distance(transform.position, temp) < 0.001f) {
                Place();
            }
        }

        else {
            transform.position = getMousePosition();
        }

        if (Input.GetMouseButtonDown(1)) {
            Remove();
        }

    }

    private Vector3 getMousePosition() {
        mousePosition = Input.mousePosition;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f;
        return worldPosition;
    }

    private bool CanPlace() {
        for (int i = 0; i < transform.Find("Blocks").childCount; i++) {
            Vector3Int cellPosition = tilemap.WorldToCell(transform.Find("Blocks").GetChild(i).transform.position);
            Vector3 temp = tilemap.GetCellCenterWorld(cellPosition);
            if (!placeablePositions.Contains(temp)) {
                return false;
            }
        }
        return true;
    }


    private void Place() {
        Instantiate(blockPrefab, transform.position + new Vector3(0f, 0.2f, 0f), transform.rotation);
        for (int i = 0; i < transform.Find("Blocks").childCount; i++) {
            Vector3Int cellPosition = tilemap.WorldToCell(transform.Find("Blocks").GetChild(i).transform.position);
            Vector3 temp = tilemap.GetCellCenterWorld(cellPosition);
            placeablePositions.Remove(temp);
            }

        CheckLevelComplete();
    }

    private void Remove() {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null && hit.collider.name != "trailerBlock") {
            Transform blocks = hit.collider.transform.parent;
            Transform parent = blocks.parent;
            for (int i = 0; i < blocks.childCount; i++) {
                Vector3Int cellPosition = tilemap.WorldToCell(blocks.GetChild(i).transform.position);
                Vector3 temp = tilemap.GetCellCenterWorld(cellPosition);
                StartCoroutine(WaitForAnimation(temp, parent));
            }
        }

    }

    private IEnumerator WaitForAnimation(Vector3 position, Transform parent) {
        yield return new WaitForSeconds(0.2f);
        parent.GetComponent<BlockMovement>().removing = true;
        placeablePositions.Add(position);

    }

    private void CheckLevelComplete() {
        if (placeablePositions.Count == 0) {
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            GameManager.Instance.NextLevel();
        }
    }
}
