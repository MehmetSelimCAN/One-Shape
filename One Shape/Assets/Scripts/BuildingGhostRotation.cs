using System.Collections;
using UnityEngine;

public class BuildingGhostRotation : MonoBehaviour {

    private Quaternion targetRotation;
    private float rotateSpeed;
    private bool isBuildingGhostRotating;
    private Transform buildingGhostTransform;
    private float targetAngle;

    private void Awake() {
        targetRotation = Quaternion.identity;
        rotateSpeed = 20f;
        buildingGhostTransform = transform;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Space)) {
            StopAllCoroutines();
            StartCoroutine(Rotate());
        }

        if (isBuildingGhostRotating) {
            targetRotation = GetTargetRotation();
            buildingGhostTransform.rotation = Quaternion.Lerp(buildingGhostTransform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    private Quaternion GetTargetRotation() {
        return Quaternion.AngleAxis(targetAngle, Vector3.forward);
    }


    private IEnumerator Rotate() {
        isBuildingGhostRotating = true;
        targetAngle += 90f;
        yield return new WaitForSeconds(0.2f);
        isBuildingGhostRotating = false;

        //Rounding to nearest 90 degree
        var tempVector = transform.eulerAngles;
        tempVector.z = Mathf.Round(tempVector.z / 90) * 90;
        transform.eulerAngles = tempVector;
    }
}
