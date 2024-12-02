using UnityEngine;

public class TridentThrown : MonoBehaviour
{
    private Rigidbody2D trident;
    public Transform originObj;
    private bool airBorne = true;
    private bool returning = false;
    private float speed = 15.0f;

    void Start()
    {
       trident = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    public void move(Vector2 direction)
    {
        trident = GetComponent<Rigidbody2D>();
        trident.linearVelocity = direction * speed;
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
        Debug.Log("Hit");
        trident.linearVelocity = Vector2.zero;
        trident.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
