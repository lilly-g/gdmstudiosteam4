using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetLevel : MonoBehaviour
{
    void OnEnable() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (null != gameObject.GetComponent<TMP_Text>()) {
            gameObject.GetComponent<TMP_Text>().text = "LEVEL: " + currentSceneIndex;
        }
    }
}
