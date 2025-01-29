using UnityEngine;

public class GrappleThrown : MonoBehaviour
{
    private Rigidbody2D grapple;
    public GrappleMovement originObj;

    private float speed = 45.0f;

    void Start()
    {
       grapple = GetComponent<Rigidbody2D>();
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
            originObj.grappled = true;
            stuck();
        }
    }

    private void stuck()
    {
        grapple.linearVelocity = Vector2.zero;
        grapple.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
