using UnityEngine;

public class DashEffect : MonoBehaviour
{
    public ParticleSystem dashEffect;

    private PlayerController playerController;
    private bool wasDashingLastFrame = false;
    private bool isDashing;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        dashEffect.Stop();
    }

    void Update()
    {
        if (playerController == null) return;

        isDashing = playerController.isDashing;

        if (isDashing)
        {
            if (!wasDashingLastFrame && dashEffect != null)
            {
                dashEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                dashEffect.Play();
            }
        }
        else
        {
            if (wasDashingLastFrame && dashEffect != null && dashEffect.isPlaying)
            {
                dashEffect.Stop();
            }
        }

        wasDashingLastFrame = isDashing;
    }
}