using TMPro;
using UnityEngine;

public class GetAttempts : MonoBehaviour
{
    private readonly string attemptsString = "attempts";
    private int attempts;

    void OnEnable() {
        attempts = PlayerPrefs.GetInt(attemptsString);
        if (null != gameObject.GetComponent<TMP_Text>()) {
            gameObject.GetComponent<TMP_Text>().text = "ATTEMPTS: " + attempts;
        }
    }
}
