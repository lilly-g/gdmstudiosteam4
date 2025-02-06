using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //make the player vanish, can instead be a vanishing animation in the future
            //so that it looks smoother
            other.gameObject.SetActive(false);
            //reload the scene
            StartCoroutine(ReloadScene());
        }
    }

    private IEnumerator ReloadScene()
    {
        yield return null; // Wait for the next frame before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


