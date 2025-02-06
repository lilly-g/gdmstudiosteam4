using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRestart : MonoBehaviour
{
    [SerializeField] private float restartThreshold;
    private float timeHeld = 0f;
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            timeHeld += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            timeHeld = 0f;
        }

        if (timeHeld >= restartThreshold)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
