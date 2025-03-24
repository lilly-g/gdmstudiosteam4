using UnityEngine;

public class PlayerParticleEffect : MonoBehaviour
{
    [SerializeField] private GameObject landEffectPrefab;  // Reference to the particle system prefab
    private bool _grounded;

    // If you have a method to detect landing (e.g., OnCollisionEnter2D or OnTriggerStay2D)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !_grounded)
        {
            // Set grounded to true to avoid multiple instantiations
            _grounded = true;

            // Instantiate the particle system at the player's position
            Instantiate(landEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Player is no longer grounded
            _grounded = false;
        }
    }
}
