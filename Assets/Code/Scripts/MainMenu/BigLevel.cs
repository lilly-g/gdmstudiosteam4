using UnityEngine;
using UnityEngine.EventSystems;

public class BigLevel : Level
{
    public static new Vector3 hoverScale = new Vector3(0.75f, 0.75f, 0.75f);
    public static new Vector3 normalScale = new Vector3(0.5f, 0.5f, 0.5f);

    public override void OnPointerEnter(PointerEventData eventData) {
        gameObject.transform.localScale = hoverScale;
    }

    public override void OnPointerExit(PointerEventData eventData) {
        gameObject.transform.localScale = normalScale;
    }
}
