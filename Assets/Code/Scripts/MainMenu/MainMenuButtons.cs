using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    [HideInInspector]
    public static bool newGamePressed = false;

    public string nextSceneName;
    public int delayAtEndOfLoad;

    public GameObject currentScreen;
    public GameObject loadingScreen;
    public Image loadingBar;

    public void NewGame() {
        newGamePressed = true;
        StartCoroutine(LoadNewScene());
    }

    IEnumerator LoadNewScene() {
        if (currentScreen.GetComponent<CanvasGroup>() != null) {
            currentScreen.GetComponent<CanvasGroup>().alpha = 0f;
            currentScreen.GetComponent<CanvasGroup>().interactable = false;
        }
        loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false;
        
        // Fill loading bar till 90% full
        while (asyncLoad.progress < 0.9f) {
            loadingBar.fillAmount = asyncLoad.progress;
            yield return null;
        }

        // Smoothly animate 90%-100% over 1 second
        float timer = 0f;
        while (timer < 1f) {
            timer += Time.deltaTime;
            loadingBar.fillAmount = Mathf.Lerp(0.9f, 1.0f, timer);
            yield return null;
        }

        yield return new WaitForSeconds(delayAtEndOfLoad);
        asyncLoad.allowSceneActivation = true;
    }

    void LoadGame() {

    }

    void Config() {

    }
}
