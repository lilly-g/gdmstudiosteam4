/*
using UnityEngine;
using UnityEngine.SceneManagement;
public class Goal : MonoBehaviour
{
    public string nextSceneName;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }
    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name is not set in the Inspector!");
        }
    }
}
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoalTrigger : MonoBehaviour
{
    public string nextSceneName; // Set this in the Inspector

    // PlayerPrefs
    private readonly string levelsCompletedString = "levelsCompleted";

    [HideInInspector] public GameObject levelLoaderCanvas;
    [HideInInspector] public Animator reload;
    private bool hasTransition;

    void Start()
    {
        levelLoaderCanvas = GameObject.FindGameObjectWithTag("Transition");
        if (levelLoaderCanvas != null){
            reload = levelLoaderCanvas.GetComponent<Animator>();
            hasTransition = true;
        }
        else {
            Debug.Log("The levelloader is missing from the scene, will reload without transition");
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetType().ToString().Equals("UnityEngine.CapsuleCollider2D")) // Ensure the player has the correct tag
        {
            int levelsCompletedInt = PlayerPrefs.GetInt(levelsCompletedString);
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentSceneIndex == levelsCompletedInt + 1) {
                levelsCompletedInt += 1;
                PlayerPrefs.SetInt(levelsCompletedString, levelsCompletedInt);
            }
            if (hasTransition){
                StartCoroutine(LoadSceneAsyncWithTransition());
            }
            else{
                StartCoroutine(LoadSceneAsync());
            }         
        }
    }

    private IEnumerator LoadSceneAsyncWithTransition()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            reload.SetTrigger("FadeIn");
            yield return new WaitForSeconds(0.45f);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
            asyncLoad.allowSceneActivation = false;
            
            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
                yield return null;
            }
        }
        else
        {
            Debug.LogWarning("Next scene name is not set in the Inspector!");
        }
    }
    private IEnumerator LoadSceneAsync()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
            asyncLoad.allowSceneActivation = false;
            
            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
                yield return null;
            }
        }
        else
        {
            Debug.LogWarning("Next scene name is not set in the Inspector!");
        }
    }
}