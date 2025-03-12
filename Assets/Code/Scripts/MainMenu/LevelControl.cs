using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private List<GameObject> levelGameObjects;

    [SerializeField] private GameObject currentScreen;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingBar;
    [SerializeField] private int delayAtEndOfLoad;
    
    // PlayerPrefs
    private string levelsCompletedString = "levelsCompleted";
    private int levelsCompletedInt;

    public void LoadLevel(int sceneIndex) {
        Debug.Log("LoadLevel called with index: " + sceneIndex);
        StartCoroutine(LoadNewScene(sceneIndex));
    }

    IEnumerator LoadNewScene(int sceneIndex) {
        if (currentScreen.GetComponent<CanvasGroup>() != null) {
            currentScreen.GetComponent<CanvasGroup>().alpha = 0f;
            currentScreen.GetComponent<CanvasGroup>().interactable = false;
        }
        loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
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

    public void AssignGameObjects() {
        levelsCompletedInt = PlayerPrefs.GetInt(levelsCompletedString);

        for (int i = 0; i < levelGameObjects.Count; i++) {
            Level level = levelGameObjects[i].AddComponent<Level>();
            level.Index = i;
            Debug.Log("Level " + (i+1) + " added.");

            levelGameObjects[i].SetActive(i <= levelsCompletedInt);
        }
    }

    void OnEnable()
    {
        AssignGameObjects();
    }
}
