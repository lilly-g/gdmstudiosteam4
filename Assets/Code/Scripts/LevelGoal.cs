using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Level Complete");
    }
}
