using UnityEngine;

public class BlockAnimator : MonoBehaviour {

    private Animator blockMovementAnimator;
    private Animator[] alphaAnimators;

    private void Awake() {
        blockMovementAnimator = GetComponent<Animator>();
        alphaAnimators = transform.GetComponentsInChildren<Animator>();
    }

    public void BlockFadeOut() {
        blockMovementAnimator.Play("BlockMovement");

        foreach (Animator alphaAnimator in alphaAnimators) {
            alphaAnimator.Play("AlphaFadeOut");
        }

        Destroy(gameObject, 0.5f);
    }
}
