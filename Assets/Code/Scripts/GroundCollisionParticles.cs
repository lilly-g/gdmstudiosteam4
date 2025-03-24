using UnityEngine;

public class GroundCollisionParticles : MonoBehaviour
{
    public ParticleSystem groundParticles;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if we hit something tagged as "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Move particle effect to the collision point and play it
            groundParticles.transform.position = collision.contacts[0].point;
            groundParticles.Play();
        }
    }
}
