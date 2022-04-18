using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockMovement : MonoBehaviour {

    private Vector3 targetPosition;
    [HideInInspector] public bool removing;
    private bool canRemove;
    private Color alphaColor;
    private float alpha = 1f;

    private Color tempColor;

    private void Awake() {
        targetPosition = GameObject.FindGameObjectWithTag("BuildingGhost").transform.position;
        tempColor = transform.Find("Blocks").GetChild(0).GetComponent<SpriteRenderer>().color;
        alphaColor = new Color(tempColor.r, tempColor.g, tempColor.b, tempColor.a);
    }

    private void Update() {
        if (canRemove && removing) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, 10f, 0f), 15f * Time.deltaTime);

            alpha -= 0.01f;
            for (int i = 0; i < transform.Find("Blocks").childCount; i++) {
                alphaColor = new Color(tempColor.r, tempColor.g, tempColor.b, alpha);
                transform.Find("Blocks").GetChild(i).GetComponent<SpriteRenderer>().color = alphaColor;
            }

            for (int i = 0; i < transform.Find("Connections").childCount; i++) {
                alphaColor = new Color(tempColor.r, tempColor.g, tempColor.b, alpha);
                transform.Find("Connections").GetChild(i).GetComponent<SpriteRenderer>().color = alphaColor;
            }

            if (Vector2.Distance(transform.position, new Vector3(targetPosition.x, 5f, 0f)) < 0.1f) {
                Destroy(gameObject);
            }
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 0.75f * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetPosition) < 0.001f) {
                canRemove = true;
            }
        }
    }
}
