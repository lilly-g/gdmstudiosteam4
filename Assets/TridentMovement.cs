using UnityEngine;

public class TridentMovement : MonoBehaviour
{
    private Vector3 mousePos;
    public Transform trident;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject tridentProjectile;

    void Start()
    {
        trident = transform.GetChild(0);
    }

   void Update()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        trident.eulerAngles = new Vector3(0, 0, angle);
        
        Debug.DrawRay(transform.position, direction * 1000, Color.red);

        if (trident.gameObject.activeSelf && Input.GetButtonDown("Fire1"))
        {
            GameObject projectile = Instantiate(tridentProjectile, transform.position, trident.rotation);
            projectile.GetComponent<TridentThrown>().move(direction);
            projectile.GetComponent<TridentThrown>().originObj = this.transform;
            //trident.gameObject.SetActive(false);
        }
        
    }
}
