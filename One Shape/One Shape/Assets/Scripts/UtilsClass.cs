using UnityEngine;
using UnityEngine.Tilemaps;

public class UtilsClass : MonoBehaviour {

    public static UtilsClass Instance { get; private set; }

    private Camera mainCamera;

    private Vector3 mousePosition;
    private Vector3 worldPosition;

    private Tilemap tilemap;
    private Vector3Int cellPosition;
    private Vector3 cellCenter;

    private void Awake() {
        Instance = this;

        mainCamera = Camera.main;
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    }

    public Vector3 GetMousePosition() {
        mousePosition = Input.mousePosition;
        worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f;
        return worldPosition;
    }

    public Vector3 GetCellCenterOnPosition(Vector3 position) {
        cellPosition = tilemap.WorldToCell(position);
        cellCenter = tilemap.GetCellCenterWorld(cellPosition);
        return cellCenter;
    }
}
