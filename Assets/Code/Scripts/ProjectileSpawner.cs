using UnityEngine;
using System.Collections.Generic;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private float projectileSpeed;
    [SerializeField] private Vector2 projectileSize;
    [SerializeField] private float spawnInterval;
    [SerializeField] private int maxProjectiles;

    private List<GameObject> projectileInstances = new List<GameObject>();
    private float frameProjectileThrown = float.MinValue;
    private float time = 0f;

    public bool isNull(GameObject obj)
    {
        return obj == null;
    }

    void Update()
    {
        time += Time.deltaTime;

        projectileInstances.RemoveAll(isNull);

        if ((time >= frameProjectileThrown + spawnInterval) && (projectileInstances.Count < maxProjectiles || maxProjectiles == -1))
        {
            ThrowProjectile();
        }
    }

    public void ThrowProjectile()
    {
        frameProjectileThrown = time;

        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectileInstance.transform.localScale = projectileSize;
        projectileInstance.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(projectileSpeed, 0);

        projectileInstances.Add(projectileInstance);
    }
}
