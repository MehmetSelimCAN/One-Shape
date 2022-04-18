using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    private Transform nextLevelAnimation;
    private Transform gameUI;
    private Transform pauseMenuUI;
    private bool paused;
    private bool nextLevel;
    private Transform grid;
    private GameObject[] shadows;
    private GameObject[] blocks;


    private void Awake() {
        Instance = this;
        gameUI = GameObject.Find("GameMenu").transform;
        pauseMenuUI = GameObject.Find("PauseMenu").transform;
        nextLevelAnimation = GameObject.Find("NextLevelAnimation").transform;
        nextLevelAnimation.gameObject.SetActive(false);
        pauseMenuUI.gameObject.SetActive(false);

        grid = GameObject.Find("Grid").transform;
        shadows = GameObject.FindGameObjectsWithTag("Shadow");

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ResumeOrPause();
        }


        if (nextLevel) {
            foreach (var block in blocks) {
                block.transform.position = Vector3.MoveTowards(block.transform.position, new Vector3(block.transform.position.x, 10f, 0f), 4f * Time.deltaTime);
            }

            grid.transform.position = Vector3.MoveTowards(grid.transform.position, new Vector3(grid.transform.position.x, -10f, 0f), 4f * Time.deltaTime);

            foreach (var shadow in shadows) {
                shadow.transform.position = Vector3.MoveTowards(shadow.transform.position, new Vector3(shadow.transform.position.x, -10f, 0f), 4f * Time.deltaTime);
            }
        }
    }


    private void ResumeOrPause() {
        if (paused) {
            paused = false;
            gameUI.gameObject.SetActive(true);
            pauseMenuUI.gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("BuildingGhost").GetComponent<Movement>().enabled = true;
            GameObject.FindGameObjectWithTag("BuildingGhost").GetComponent<Rotation>().enabled = true;
        }
        else {
            paused = true;
            gameUI.gameObject.SetActive(false);
            pauseMenuUI.gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("BuildingGhost").GetComponent<Movement>().enabled = false;
            GameObject.FindGameObjectWithTag("BuildingGhost").GetComponent<Rotation>().enabled = false;
        }
    }

    public void NextLevel() {
        PlayerPrefs.SetInt("LastLevel", SceneManager.GetActiveScene().buildIndex + 2);
        StartCoroutine(WaitForAnimation());
    }

    private IEnumerator WaitForAnimation() {
        blocks = GameObject.FindGameObjectsWithTag("Block");
        nextLevel = true;
        nextLevelAnimation.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
