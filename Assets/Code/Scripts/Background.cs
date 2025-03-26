using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private GameObject sky;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject foreground;
    [SerializeField] private GameObject foreBelow;
    [SerializeField] private float foreOffset;
    [SerializeField] private float foreBelowOffset;
    [SerializeField] private float foreParallax;
    [SerializeField] private float backParallax;
    private Camera mainCamera;

    void Start(){
        mainCamera = Camera.main;
    }

    void Update(){
        if (sky != null){
            sky.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);
        }
        if (foreground != null){
            foreground.transform.position = new Vector3(mainCamera.transform.position.x, (mainCamera.transform.position.y * foreParallax) - foreOffset, 0);
        }
        if (foreBelow != null){
            foreBelow.transform.position = new Vector3(mainCamera.transform.position.x, Mathf.Min(foreBelowOffset, mainCamera.transform.position.y), 0);
        }
        if (background != null){
            background.transform.position = new Vector3(mainCamera.transform.position.x, Mathf.Min(mainCamera.transform.position.y, mainCamera.transform.position.y * backParallax), 0);
        }
    }
}
