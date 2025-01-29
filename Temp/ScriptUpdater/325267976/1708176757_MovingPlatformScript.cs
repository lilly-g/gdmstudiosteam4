using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    public Transform Left, Right;
    public int speed;
    public Rigidbody2D platformRb;
    private Vector2 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = Right.position; // Start by moving right
        platformRb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, Left.position) < .1f)
        {
            direction = Right.position;
        }

        if (Vector2.Distance(transform.position, Right.position) < .1f)
        {
            direction = Left.position;
        }

        // Move the platform using velocity instead of MovePosition
        Vector2 velocity = (direction - (Vector2)transform.position).normalized * speed;
        platformRb.linearVelocity = new Vector2(velocity.x, platformRb.linearVelocity.y);  // Keep the Y velocity unchanged to maintain gravity
    }

    // Make it so the player can stand on the platform
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    // Player no longer touching platform
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
