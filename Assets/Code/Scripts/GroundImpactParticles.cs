using UnityEngine;

public class GroundImpactParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem groundParticles;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (groundParticles != null)
            {
                // Move particle system to collision point
                groundParticles.transform.position = collision.contacts[0].point;

                // Restart the particle effect
                groundParticles.Play();
            }
        }
    }
}
