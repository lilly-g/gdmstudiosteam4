using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PausedOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static readonly Color32 normalBarColor = new Color32(0, 100, 0, 0);
    public static readonly Color32 hoverBarColor = new Color32(0, 100, 0, 255);

    public static readonly Color32 normalBulletColor = new Color32(210, 190, 170, 255);
    public static readonly Color32 hoverBulletColor = new Color32(80, 60, 40, 255);

    public static readonly Color32 normalTextColor = new Color32(25, 100, 25, 255);
    public static readonly Color32 hoverTextColor = new Color32(255, 255, 255, 255);

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject text;

    public void OnPointerEnter(PointerEventData eventData) {
        gameObject.GetComponent<Image>().color = hoverBarColor;
        bullet.GetComponent<Image>().color = hoverBulletColor;
        text.GetComponent<TMP_Text>().color = hoverTextColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
        gameObject.GetComponent<Image>().color = normalBarColor;
        bullet.GetComponent<Image>().color = normalBulletColor;
        text.GetComponent<TMP_Text>().color = normalTextColor;
    }

    void OnEnable() {
        gameObject.GetComponent<Image>().color = normalBarColor;
        bullet.GetComponent<Image>().color = normalBulletColor;
        text.GetComponent<TMP_Text>().color = normalTextColor;
    }
}
