using UnityEngine;
using UnityEngine.UI.Extensions;

public class LevelMap : Indexed
{
    [SerializeField] private GameObject arrowLeft;
    [SerializeField] private GameObject arrowRight;

    public GameObject ArrowLeft {
        get {return arrowLeft;}
    }

    public GameObject ArrowRight {
        get {return arrowRight;}
    }

    public void CallGoLeft() {
        if (gameObject.GetComponentInParent<LevelControl>() != null) {
            gameObject.GetComponentInParent<LevelControl>().goLeft();
        }
        else {
            Debug.LogWarning("No LevelControl found!");
        }
    }

    public void CallGoRight() {
        if (gameObject.GetComponentInParent<LevelControl>() != null) {
            gameObject.GetComponentInParent<LevelControl>().goRight();
        }
        else {
            Debug.LogWarning("No LevelControl found!");
        }
    }
}
