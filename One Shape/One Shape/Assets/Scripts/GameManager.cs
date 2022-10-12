using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    private Transform gameUI;
    private Transform pauseMenuUI;
    private bool isGamePaused;

    private Transform blocksParent;
    private Transform shadowsParent;

    private Animator nextLevelAnimator;
    private Animator gridAnimator;

    private Transform buildingGhost;
    private BuildingGhostMovement buildingGhostMovement;
    private BuildingGhostRotation buildingGhostRotation;

    private void Awake() {
        Instance = this;

        gameUI = GameObject.Find("GameMenu").transform;
        pauseMenuUI = GameObject.Find("PauseMenu").transform;
        pauseMenuUI.gameObject.SetActive(false);

        blocksParent = GameObject.Find("Blocks").transform;
        shadowsParent = GameObject.Find("Shadows").transform;

        gridAnimator = GameObject.Find("Grid").transform.GetComponent<Animator>();
        nextLevelAnimator = GameObject.Find("NextLevelAnimation").transform.GetComponent<Animator>();


        buildingGhost = GameObject.Find("BuildingGhost").transform;
        buildingGhostMovement = buildingGhost.GetComponent<BuildingGhostMovement>();
        buildingGhostRotation = buildingGhost.GetComponent<BuildingGhostRotation>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isGamePaused)       ResumeGame();
            else                    PauseGame();
        }
    }

    private void ResumeGame() {
        isGamePaused = false;
        gameUI.gameObject.SetActive(true);
        pauseMenuUI.gameObject.SetActive(false);

        ActiveBuildingGhostScripts();
    }

    private void ActiveBuildingGhostScripts() {
        buildingGhostMovement.enabled = true;
        buildingGhostRotation.enabled = true;
    }

    private void PauseGame() {
        isGamePaused = true;
        gameUI.gameObject.SetActive(false);
        pauseMenuUI.gameObject.SetActive(true);

        DeactiveBuildingGhostScripts();
    }

    private void DeactiveBuildingGhostScripts() {
        buildingGhostMovement.enabled = false;
        buildingGhostRotation.enabled = false;
    }

    public void NextLevel() {
        PlayerPrefs.SetInt("LastLevel", SceneManager.GetActiveScene().buildIndex + 2);
        StartCoroutine(WaitForAnimation());
    }

    private IEnumerator WaitForAnimation() {
        nextLevelAnimator.Play("NextLevel");

        gridAnimator.Play("GridMovement");

        for (int i = 0; i < blocksParent.childCount; i++) {
            blocksParent.GetChild(i).GetComponent<BlockAnimator>().BlockFadeOut();
        }

        for (int i = 0; i < shadowsParent.childCount; i++) {
            shadowsParent.GetChild(i).GetComponent<Animator>().Play("ShadowFadeOut");
        }

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
