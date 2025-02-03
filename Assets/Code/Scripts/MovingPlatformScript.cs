using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform Left, Right;
    [SerializeField] private float speed;
    private float direction;
    private Rigidbody2D _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = speed; //it starts by moving right
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, Left.position) < .1f) { 
            direction = speed;
        }
        
        if (Vector2.Distance(transform.position, Right.position) < .1f) { 
            direction = -speed;
        }

        _rb.linearVelocity = new Vector2 (direction, _rb.linearVelocity.y);
    }

    //make it so player can stand on the platform
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Debug.Log("enter platform");
            collision.gameObject.GetComponent<PlayerController>()._platform = this;
        }
    }

    //player no longer touching platform
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Debug.Log("exit platform");
            collision.gameObject.GetComponent<PlayerController>()._platform = null;
        }
    }

    public float getDirection()
    {
        return direction;
    }

}
