using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockPlacement : MonoBehaviour {

    public static BlockPlacement Instance { get; private set; }

    private Tilemap tilemap;
    private List<Vector3> placeablePositions;
    private Vector3 cellCenter;
    private Vector3Int localPlace;

    public Transform blockPrefab;
    private Transform blocksParent;

    private Transform buildingGhostBlocks;

    private void Awake() {
        Instance = this;

        blocksParent = GameObject.Find("Blocks").transform;

        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        buildingGhostBlocks = transform.Find("BuildingGhostBlocks");

        placeablePositions = FindPlaceablePositions();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            PlaceBlock();
        }

        else if (Input.GetMouseButtonDown(1)) {
            RemoveBlock();
        }
    }

    private List<Vector3> FindPlaceablePositions() {
        List<Vector3> tempPlaceablePositions = new List<Vector3>();
        for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++) {
            for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++) {
                localPlace = new Vector3Int(n, p, (int)tilemap.transform.position.y);
                cellCenter = tilemap.GetCellCenterWorld(localPlace);
                if (tilemap.HasTile(localPlace)) {
                    tempPlaceablePositions.Add(cellCenter);
                }
            }
        }

        return tempPlaceablePositions;
    }

    public List<Vector3> GetPlaceablePositions() {
        return placeablePositions;
    }

    private bool CanPlace() {
        cellCenter = UtilsClass.Instance.GetCellCenterOnPosition(UtilsClass.Instance.GetMousePosition());
        //If the building ghost is not in the center of the cell, we can't place.
        if (Vector3.Distance(transform.position, cellCenter) > 0.001f) {
            return false;
        }

        //If we don't have placeable position for every block, we can't place.
        for (int i = 0; i < buildingGhostBlocks.childCount; i++) {
            Vector3Int cellPosition = tilemap.WorldToCell(buildingGhostBlocks.GetChild(i).transform.position);
            Vector3 childBlockWorldPosition = tilemap.GetCellCenterWorld(cellPosition);
            if (!placeablePositions.Contains(childBlockWorldPosition)) {
                return false;
            }
        }

        return true;
    }

    private void PlaceBlock() {
        if (CanPlace()) {
            Instantiate(blockPrefab, transform.position, transform.rotation, blocksParent);

            //Remove all blocks' positions from placeable positions list.
            for (int i = 0; i < buildingGhostBlocks.childCount; i++) {
                Transform childBlock = buildingGhostBlocks.GetChild(i).transform;
                cellCenter = UtilsClass.Instance.GetCellCenterOnPosition(childBlock.position);
                placeablePositions.Remove(cellCenter);
                BuildingGhostMovement.Instance.SetPlaceablePositions(placeablePositions);
            }

            //Check all placeable positions after placing.
            CheckLevelComplete();
        }
    }

    private void RemoveBlock() {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null) {
            Transform blocks = hit.collider.transform.parent;
            Transform parent = blocks.parent;
            for (int i = 0; i < blocks.childCount; i++) {
                Vector3Int cellPosition = tilemap.WorldToCell(blocks.GetChild(i).transform.position);
                Vector3 cellCenter = tilemap.GetCellCenterWorld(cellPosition);
                StartCoroutine(WaitForAnimation(cellCenter, parent));
            }
        }
    }

    private IEnumerator WaitForAnimation(Vector3 position, Transform parent) {
        parent.GetComponent<BlockAnimator>().BlockFadeOut();
        yield return new WaitForSeconds(0.2f);
        placeablePositions.Add(position);
        BuildingGhostMovement.Instance.SetPlaceablePositions(placeablePositions);
    }

    private void CheckLevelComplete() {
        if (placeablePositions.Count == 0) {
            for (int i = 0; i < transform.childCount; i++) {
                //Deactive building ghost
                transform.GetChild(i).gameObject.SetActive(false);
            }

            GameManager.Instance.NextLevel();
        }
    }
}
