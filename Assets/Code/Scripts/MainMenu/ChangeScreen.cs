using UnityEngine;

public class ChangeScreen : MonoBehaviour
{
    [SerializeField] private GameObject screenToLoad;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Backspace)) {
            GoToCanvas(screenToLoad);
        }
    }

    public void GoToCanvas(GameObject aCanvas) {
        aCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
}
