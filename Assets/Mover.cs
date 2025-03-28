using UnityEngine;

public class Mover : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Transform target;

    void Start()
    {
        target = pointB; // Start by moving toward point B
    }

    void Update()
    {
        if (pointA == null || pointB == null) return;

        // Move toward the target point
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // If close enough to the target, switch direction
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            target = (target == pointA) ? pointB : pointA;
        }
    }
}
