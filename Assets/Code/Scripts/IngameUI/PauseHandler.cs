using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class PauseHandler : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public static int PreviousLevelIndex { get; private set; }

    private readonly string attemptsString = "attempts";
    private int attempts;

    private bool paused;

    void Start()
    {
        Debug.Log("scene loaded!");

        attempts = PlayerPrefs.GetInt(attemptsString);
        if (SceneManager.GetActiveScene().buildIndex == PreviousLevelIndex + 1) attempts = 0;
        attempts++;
        PlayerPrefs.SetInt(attemptsString, attempts);
    }

    void OnDestroy() {
        PreviousLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            switch (paused) {
                case true:
                    Resume();
                    break;
                case false:
                    Pause();
                    break;
            }
        }
    }

    public void Resume() { 
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    public void Pause() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void Restart() {
        Resume();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu() {
        Resume();
        SceneManager.LoadSceneAsync(0);
    }
}
