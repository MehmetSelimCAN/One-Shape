using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    private void Awake() {
        if (PlayerPrefs.GetInt("LastLevel") == 0) {
            PlayerPrefs.SetInt("LastLevel", 1);
        }

        for (int i = 0; i < PlayerPrefs.GetInt("LastLevel"); i++) {
            int x = i;
            Image buttonImage = transform.GetChild(x).GetComponent<Image>();
            Color alphaColor = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1);
            transform.GetChild(x).GetComponent<Image>().color = alphaColor;

            transform.GetChild(x).GetComponent<Button>().onClick.AddListener(() => {
                SceneManager.LoadScene(transform.GetChild(x).name);
            });
        }
    }
}
