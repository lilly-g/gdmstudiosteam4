using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    public Transform Left, Right;
    public int speed;
    Vector2 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = Right.position; //it starts by moving right
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, Left.position) < .1f) { 
            direction = Right.position;
        }
        
        if (Vector2.Distance(transform.position, Right.position) < .1f) { 
            direction = Left.position;
        }

        transform.position = Vector2.MoveTowards(transform.position,direction,speed * Time.deltaTime);

        
    }

    //make it so player can stand on the platform
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Debug.Log("enter platform");
            collision.transform.SetParent(this.transform);
        }
    }

    //player no longer touching platform
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Debug.Log("exit platform");
            collision.transform.SetParent(null);
        }
    }

}
