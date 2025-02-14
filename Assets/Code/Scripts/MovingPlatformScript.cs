using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform Left, Right;
    [SerializeField] private float speed;
    [SerializeField] private float waitTime;
    [SerializeField] private float acceleration;
    [SerializeField] private bool accelerates;

    private float direction;
    private Rigidbody2D _rb;

    void Start()
    {
        direction = speed; //it starts by moving right
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if ((Vector2.Distance(transform.position, Left.position) < .1f) && direction == -speed) {
            direction = 0f;
            IEnumerator changeDir = ChangeDirection(speed);
            StartCoroutine(changeDir);
        }
        else if ((Vector2.Distance(transform.position, Right.position) < .1f) && direction == speed) {
            direction = 0f;
            IEnumerator changeDir = ChangeDirection(-speed);
            StartCoroutine(changeDir);
        }

        _rb.linearVelocity = new Vector2 (accelerates ? Mathf.MoveTowards(_rb.linearVelocity.x, direction, acceleration * Time.fixedDeltaTime) : direction, _rb.linearVelocity.y);
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

    IEnumerator ChangeDirection(float dir)
    {
        yield return new WaitForSeconds(waitTime);
        direction = dir;
    }

    public float getDirection()
    {
        return direction;
    }

}
