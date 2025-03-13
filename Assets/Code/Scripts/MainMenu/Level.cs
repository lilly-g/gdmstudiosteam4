using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Level : Indexed, IPointerEnterHandler, IPointerExitHandler
{
    public static Vector3 hoverScale = new Vector3(0.5f, 0.5f, 0.5f);
    public static Vector3 normalScale = new Vector3(0.25f, 0.25f, 0.25f);
    
    private Button myButton;

    public virtual void OnPointerEnter(PointerEventData eventData) {
        gameObject.transform.localScale = hoverScale;
    }

    public virtual void OnPointerExit(PointerEventData eventData) {
        gameObject.transform.localScale = normalScale;
    }

    void Start() {
        // Get the Button component from the same GameObject
        myButton = GetComponent<Button>();

        if (myButton != null)
        {
            myButton.onClick.RemoveAllListeners();
            myButton.onClick.AddListener(CallLoadLevel);
        }
        else
        {
            Debug.LogError("No Button component found on this GameObject!");
        }
    }

    void CallLoadLevel() {
        gameObject.GetComponentInParent<LevelControl>().LoadLevel(Index + 1);
    }
}
