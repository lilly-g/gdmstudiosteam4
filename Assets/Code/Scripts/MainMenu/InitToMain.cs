using UnityEngine;

public class Init_to_Main : MonoBehaviour {
    public GameObject nextObject;
    private bool keyPressed = false;

    void Update() {
        if (Input.anyKey && !keyPressed) {
            Debug.Log(Input.inputString);
            keyPressed = true;
        }
        
        if (keyPressed) {
            // Replace this with an animation later on
            gameObject.SetActive(false);
            nextObject.SetActive(true);
            enabled = false;
        }
    }
}
