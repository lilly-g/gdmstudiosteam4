using UnityEngine;

public class Collectable : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        //player has multiple colliders, checks if collider is capsule to ensure collectable only interacts with one player collider
        if (other.gameObject.CompareTag("Player") && other.GetType().ToString().Equals("UnityEngine.CapsuleCollider2D"))
        {
            gameObject.SetActive(false);
        }
    }
}
