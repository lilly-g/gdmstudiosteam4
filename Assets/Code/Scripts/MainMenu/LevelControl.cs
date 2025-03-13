using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private List<GameObject> levelGameObjects;
    [SerializeField] private List<GameObject> levelMapGameObjects;
    [SerializeField] private GameObject currentScreen;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingBar;
    [SerializeField] private int delayAtEndOfLoad;

    public static int BIG_LEVEL_THRESHOLD = 16;
    public static int LEVELS_PER_MAP = 8;
    
    // PlayerPrefs
    private string levelsCompletedString = "levelsCompleted";
    private int levelsCompletedInt;

    private int currentMapPage;

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

    private T attachLevelObject<T>(GameObject levelGameObject) where T : Indexed { 
        T level = levelGameObject.AddComponent<T>();
        return level;
    }

    public void AssignGameObjects() {
        levelsCompletedInt = 18; 

        for (int i = 0; i < levelGameObjects.Count; i++) {
            if (i < BIG_LEVEL_THRESHOLD) {
                attachLevelObject<Level>(levelGameObjects[i]).Index = i;
            }
            else {
                attachLevelObject<BigLevel>(levelGameObjects[i]).Index = i;
            }
            Debug.Log("Level " + (i+1) + " added.");

            levelGameObjects[i].SetActive(i <= levelsCompletedInt);
        }

        for (int i = 0; i < levelMapGameObjects.Count; i++) {
            levelMapGameObjects[i].GetComponent<LevelMap>().ArrowLeft.SetActive(i != 0);

            levelMapGameObjects[i].GetComponent<LevelMap>().ArrowRight.SetActive(i != levelMapGameObjects.Count - 1);
        }

        currentMapPage = levelsCompletedInt / LEVELS_PER_MAP;
        levelMapGameObjects[currentMapPage].SetActive(true);
    }

    public void goRight() {
        currentMapPage += 1;
        levelMapGameObjects[currentMapPage].SetActive(true);
        levelMapGameObjects[currentMapPage - 1].SetActive(false);
    }
    
    public void goLeft() {
        currentMapPage -= 1;
        levelMapGameObjects[currentMapPage].SetActive(true);
        levelMapGameObjects[currentMapPage + 1].SetActive(false);

    }
    void OnEnable()
    {
        AssignGameObjects();
    }
}
