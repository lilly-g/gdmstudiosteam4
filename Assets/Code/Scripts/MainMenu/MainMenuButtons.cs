using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : PopupWithPrompt // Inherits popups and GoToCanvas
{
    // UI, QOL
    [SerializeField] private GameObject currentScreen;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingBar;
    [SerializeField] private int delayAtEndOfLoad;
    [SerializeField] private GameObject levelSelector;
    [SerializeField] private GameObject configMenu;
    [SerializeField] private GameObject loadButton;

    // PlayerPrefs
    private string levelsCompletedString = "levelsCompleted";
    private int levelsCompletedInt;

    // Checks
    private static bool newGamePressed = false;

    // Getters
    public static bool GetNewGamePressed() {
        return newGamePressed;
    }

    // Setters
    public static void SetNewGamePressed() {
        newGamePressed = false;
    }

    // Inheritance
    public override void YesAction()
    {
        SetNewGamePressed();
        PlayerPrefs.SetInt(levelsCompletedString, 0);
        levelsCompletedInt = PlayerPrefs.GetInt(levelsCompletedString);
        StartCoroutine(LoadNewScene(levelsCompletedInt + 1));
    }

    public override void NoAction()
    {
        SetNewGamePressed();
    }

    // Button methods
    public void NewGame() {
        newGamePressed = true;
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

    public void GoToLevelSelector() {
        GoToCanvas(levelSelector);
    }

    public void GoToConfigMenu() {
        GoToCanvas(configMenu);
    }

    void OnEnable()
    {
        if (!PlayerPrefs.HasKey(levelsCompletedString)) {
            PlayerPrefs.SetInt(levelsCompletedString, 0);
        }
        levelsCompletedInt = PlayerPrefs.GetInt(levelsCompletedString);

        // if (levelsCompletedInt == 0) loadButton.SetActive(false);
    }

    void Update()
    {
        setToggle(newGamePressed);
        setYes(Input.GetKeyUp(KeyCode.Y));
        setNo(Input.GetKeyUp(KeyCode.N));

        if (!QuitGame.GetQuitStatus()) {
            ToggleUI();
        }
    }
}