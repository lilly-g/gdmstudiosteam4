using System;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public GameObject quitUI;
    public float transparencyAlpha;
    private bool quitConfirm = false;

    // Update is called once per frame
    void Update() {
        if (!MainMenuButtons.newGamePressed) {
            if (!quitConfirm && Input.GetKeyUp(KeyCode.Escape)) {
                quitConfirm = true;
            }

            quitUI.SetActive(quitConfirm);

            switch (quitConfirm)
            {
                case true:
                    if (gameObject.GetComponent<CanvasGroup>() != null) {
                        gameObject.GetComponent<CanvasGroup>().alpha = transparencyAlpha;
                        gameObject.GetComponent<CanvasGroup>().interactable = false;
                    }
                    break;
                case false:
                    if (gameObject.GetComponent<CanvasGroup>() != null) {
                        gameObject.GetComponent<CanvasGroup>().alpha = 1f;
                        gameObject.GetComponent<CanvasGroup>().interactable = true;
                    }
                    break;
            }

            if (quitConfirm) {
                if (Input.GetKeyUp(KeyCode.Y)) {
                    Quit();
                }

                if (Input.GetKeyUp(KeyCode.N)) {
                    quitConfirm = false;
                }
            }
        }
    }

    void Quit() {
        Application.Quit();

        // Ensure it stops playing in the Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
