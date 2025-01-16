using UnityEngine;

public class GrappleThrown : MonoBehaviour
{
    private Rigidbody2D grapple;
    public Transform originObj;
    private bool airBorne = true;
    private float speed = 45.0f;

    void Start()
    {
       grapple = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    public void move(Vector2 direction)
    {
        grapple = GetComponent<Rigidbody2D>();
        grapple.linearVelocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            airBorne = false;
            stuck();
        }
    }

    private void stuck()
    {
        grapple.linearVelocity = Vector2.zero;
        grapple.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public bool IsAirBorne()
    {
        return airBorne;
    }
}
