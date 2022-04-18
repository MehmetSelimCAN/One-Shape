using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerBlock : MonoBehaviour {

    private bool removing;
    private RaycastHit2D hit;
    private Transform buildingGhost;

    private void Awake() {
        buildingGhost = GameObject.FindGameObjectWithTag("BuildingGhost").transform;
        for (int i = 0; i < buildingGhost.childCount; i++) {
            buildingGhost.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void Update() {
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null) {
            if (hit.collider.name == this.name && Input.GetMouseButtonDown(1)) {
                removing = true;
            }
        }

        if (removing) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 10f, 0f), 10f * Time.deltaTime);

            for (int i = 0; i < buildingGhost.childCount; i++) {
                buildingGhost.GetChild(i).gameObject.SetActive(true);
            }

            if (Vector2.Distance(transform.position, new Vector3(transform.position.x, 5f, 0f)) < 0.1f) {
                Destroy(gameObject);
            }
        }
    }


}
