using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject spawn; // The prefab to spawn
    [SerializeField] private ParticleSystem groundHitParticles; // Assign in Inspector

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1);
    }

    void Start()
    {
        if (spawn != null)
        {
            GameObject spawnedCharacter = Instantiate(spawn, transform.position, Quaternion.identity);

            if (groundHitParticles != null)
            {
                groundHitParticles.transform.SetParent(spawnedCharacter.transform);
            }
        }
    }
}
