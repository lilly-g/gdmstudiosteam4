using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground") && other.name != "Platform")
            {
                Destroy(this.gameObject);
            }
    }
}
