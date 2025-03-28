using UnityEngine;

public class PlayerParticleEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;

    private PlayerController playerController;
    private bool wasGrapplingLastFrame = false;
    
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        particleSystem.Stop();
    }

    void Update()
    {
        if (playerController == null || particleSystem == null) return;

        if (playerController._grapple.grappleRope.isGrappling)
        {
            if (!wasGrapplingLastFrame){
                particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                particleSystem.Play();
            }
        }
        else if (wasGrapplingLastFrame && particleSystem.isPlaying)
        {
            particleSystem.Stop();
        }

        wasGrapplingLastFrame = playerController._grapple.grappleRope.isGrappling;
    }
}
