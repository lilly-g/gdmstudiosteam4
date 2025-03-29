using UnityEngine;
using System.Collections;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private Transform Left, Right;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float waitTime;
    [SerializeField] private float radius;
    [SerializeField] private bool circleMove;

    private float direction;
    private float currentAngle;
    private Rigidbody2D _rb;

    void Start()
    {
        direction = speed; //it starts by moving right
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (circleMove){
            currentAngle += speed * Time.deltaTime;
            transform.localPosition = new Vector3(Mathf.Sin(currentAngle), Mathf.Cos(currentAngle), 0) * radius;
        }
        else{
            if (transform.position.x <= Left.position.x && direction == -speed) {
                direction = 0f;
                IEnumerator changeDir = ChangeDirection(speed);
                StartCoroutine(changeDir);
            }
            else if (transform.position.x >= Right.position.x && direction == speed) {
                direction = 0f;
                IEnumerator changeDir = ChangeDirection(-speed);
                StartCoroutine(changeDir);
            }
        
            _rb.linearVelocity = new Vector2 (Mathf.MoveTowards(_rb.linearVelocity.x, direction, acceleration * Time.fixedDeltaTime), _rb.linearVelocity.y);
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
