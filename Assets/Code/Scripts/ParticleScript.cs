using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    private static ParticleScript instance;

    public ParticleSystem grappleParticles;
    public ParticleSystem dashParticles;
    public ParticleSystem groundHitParticles;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep object across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    public void PlayGrappleEffect(Vector3 position)
    {
        grappleParticles.transform.position = position;
        grappleParticles.Play();
    }

    public void PlayDashEffect(Vector3 position)
    {
        dashParticles.transform.position = position;
        dashParticles.Play();
    }

    public void PlayGroundHitEffect(Vector3 position)
    {
        groundHitParticles.transform.position = position;
        groundHitParticles.Play();
    }
}
