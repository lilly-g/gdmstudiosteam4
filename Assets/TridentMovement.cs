using UnityEngine;

public class TridentMovement : MonoBehaviour
{
    private Vector3 mousePos;
    private GameObject tridentThrown;
    private bool thrown;
    public Transform tridentHeld;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject tridentProjectile;

    void Start()
    {
        tridentHeld = transform.GetChild(0);
    }

   void Update()
    {

        //Get mouse position and find direction between player and mouse
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;

        //Calculate angle and rotate trident
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        tridentHeld.eulerAngles = new Vector3(0, 0, angle);

        if (tridentHeld.gameObject.activeSelf && Input.GetButtonDown("Fire1"))
        {
            tridentThrown = Instantiate(tridentProjectile, transform.position, tridentHeld.rotation);
            tridentThrown.GetComponent<TridentThrown>().move(direction);
            tridentThrown.GetComponent<TridentThrown>().originObj = this.transform;
            //trident.gameObject.SetActive(false);
        }
        
    }
}
