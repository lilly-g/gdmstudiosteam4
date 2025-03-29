using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectable : MonoBehaviour
{
    private readonly string collectiblesCollectedString = "collectiblesCollected";
    private int collectiblesCollectedInt;

    private string collectibleForLevelString = "collectibleForLevel";

    void OnEnable() {
        collectiblesCollectedInt = PlayerPrefs.GetInt(collectiblesCollectedString);

        collectibleForLevelString += SceneManager.GetActiveScene().buildIndex;

        if (PlayerPrefs.GetInt(collectibleForLevelString) == 1) gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //player has multiple colliders, checks if collider is capsule to ensure collectable only interacts with one player collider
        if (other.gameObject.CompareTag("Player") && other.GetType().ToString().Equals("UnityEngine.CapsuleCollider2D"))
        {
            collectiblesCollectedInt++;
            PlayerPrefs.SetInt(collectiblesCollectedString, collectiblesCollectedInt);

            PlayerPrefs.SetInt(collectibleForLevelString, 1);
            
            gameObject.SetActive(false);
        }
    }
}
