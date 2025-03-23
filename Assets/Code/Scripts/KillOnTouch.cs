using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.Splines.Interpolators;
using Unity.Mathematics;

public class KillOnTouch : MonoBehaviour
{
    public GameObject levelLoaderCanvas;
    public Animator reload;
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
        if (other.CompareTag("Player"))
        {
            //make the player vanish, can instead be a vanishing animation in the future
            //so that it looks smoother
            other.gameObject.SetActive(false);
            //reload the scene
            if (hasTransition){
                 StartCoroutine(ReloadSceneWithTransition());
            }
            else{
                 StartCoroutine(ReloadScene());
            }
        }
    }

    private IEnumerator ReloadSceneWithTransition()
    {
        reload.SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.45f); // Wait for the next frame before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private IEnumerator ReloadScene()
    {
        yield return null; // Wait for the next frame before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


