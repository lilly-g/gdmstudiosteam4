using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player is kill");
            Destroy(this.gameObject);
            
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
    }
}
