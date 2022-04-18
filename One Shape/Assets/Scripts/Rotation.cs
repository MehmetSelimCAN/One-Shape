using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    private Quaternion targetRotation = Quaternion.identity;
    private float rotateSpeed = 20f;
    private bool rotating;
    private float angle;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Space) && !rotating) {
            StopAllCoroutines();
            StartCoroutine(Rotate());
        }

        if (rotating) {
            targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            
        }
    }


    private IEnumerator Rotate() {
        rotating = true;
        angle += 90f;
        yield return new WaitForSeconds(0.2f);
        rotating = false;
        var tempVector = transform.eulerAngles;
        tempVector.z = Mathf.Round(tempVector.z / 90) * 90;
        transform.eulerAngles = tempVector;
    }
}
