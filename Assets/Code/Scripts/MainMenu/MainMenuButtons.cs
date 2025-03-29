using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : PopupWithPrompt // Inherits popups and GoToCanvas
{
    // UI, QOL
    [SerializeField] private GameObject currentScreen;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingBar;
    [SerializeField] private int delayAtEndOfLoad = 1;
    [SerializeField] private GameObject levelSelector;
    [SerializeField] private GameObject configMenu;
    [SerializeField] private GameObject tutorialMenu;
    [SerializeField] private GameObject loadButton;

    // PlayerPrefs
    private readonly string levelsCompletedString = "levelsCompleted";
    private int levelsCompletedInt;

    private readonly string collectiblesCollectedString = "collectiblesCollected";
    private int collectiblesCollectedInt;

    private readonly string attemptsString = "attempts";

    private readonly string collectibleForLevelString = "collectibleForLevel";

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
        // Reset completed levels
        PlayerPrefs.SetInt(levelsCompletedString, 0);
        levelsCompletedInt = PlayerPrefs.GetInt(levelsCompletedString);
        // Reset collectibles
        PlayerPrefs.SetInt(collectiblesCollectedString, 10);
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(LoadNewScene(levelsCompletedInt + 1));
        base.YesAction();
    }

    public override void NoAction()
    {
        SetNewGamePressed();
        EventSystem.current.SetSelectedGameObject(null);
        base.NoAction();
    }

    // Button methods
    public void NewGame() {
        newGamePressed = true;
    }

    private IEnumerator LoadNewScene(int sceneIndex) {
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
            timer += Time.unscaledDeltaTime;
            loadingBar.fillAmount = Mathf.Lerp(0.9f, 1.0f, timer);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(delayAtEndOfLoad);
        loadingBar.fillAmount = 0.0f;
        asyncLoad.allowSceneActivation = true;
    }

    public void GoToLevelSelector() {
        GoToCanvas(levelSelector);
    }

    public void GoToConfigMenu() {
        GoToCanvas(configMenu);
    }

    public void GoToTutorialMenu() {
        GoToCanvas(tutorialMenu);
    }

    void OnEnable()
    {
        if (!PlayerPrefs.HasKey(levelsCompletedString)) {
            PlayerPrefs.SetInt(levelsCompletedString, 0);
        }
        levelsCompletedInt = PlayerPrefs.GetInt(levelsCompletedString);

        if (levelsCompletedInt == 0) loadButton.SetActive(false);

        PlayerPrefs.SetInt(attemptsString, 0);

        if (!PlayerPrefs.HasKey(collectiblesCollectedString)) {
            PlayerPrefs.SetInt(collectiblesCollectedString, 10);
        }
        collectiblesCollectedInt = PlayerPrefs.GetInt(collectiblesCollectedString);

        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++) {
            if (!PlayerPrefs.HasKey(collectibleForLevelString + i)) {
                PlayerPrefs.SetInt(collectibleForLevelString + i, 0);
            }
        }
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