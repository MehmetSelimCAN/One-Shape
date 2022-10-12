using System.Collections.Generic;
using UnityEngine;

public class BuildingGhostMovement : MonoBehaviour {

    public static BuildingGhostMovement Instance { get; private set; }

    private List<Vector3> placeablePositions;
    private Vector3 cellCenter;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        placeablePositions = BlockPlacement.Instance.GetPlaceablePositions();
    }

    public void SetPlaceablePositions(List<Vector3> placeablePositions) {
        this.placeablePositions = placeablePositions;
    }

    private void Update() {
        //Finding cell center on the mouse position
        cellCenter = UtilsClass.Instance.GetCellCenterOnPosition(UtilsClass.Instance.GetMousePosition());

        //If we are close enough to empty cell center, it automatically place itself to cell center.
        if (Vector3.Distance(transform.position, cellCenter) < 0.25f && placeablePositions.Contains(cellCenter)) {
            transform.position = Vector3.MoveTowards(transform.position, cellCenter, 3f * Time.deltaTime);
        }

        //If we are not close enough, building ghost goes to mouse position.
        else {
            transform.position = UtilsClass.Instance.GetMousePosition();
        }
    }
}
