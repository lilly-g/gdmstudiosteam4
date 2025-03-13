using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private GameObject sky;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject foreground;
    private Camera mainCamera;

    void Start(){
        mainCamera = Camera.main;
    }

    void Update(){
        sky.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);

        foreground.transform.position = new Vector3(mainCamera.transform.position.x, Mathf.Min(mainCamera.transform.position.y - 6.8f, ((mainCamera.transform.position.y * 0.25f) - 3.4f)), 0);
        background.transform.position = new Vector3(mainCamera.transform.position.x, Mathf.Min(mainCamera.transform.position.y, mainCamera.transform.position.y * 0.5f), 0);
    }
}
