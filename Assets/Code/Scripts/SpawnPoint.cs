using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject spawn;
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1);
    }

    void Start()
    {
        if (spawn != null)
        {
            Instantiate(spawn, transform.position, Quaternion.identity);
        }
    }
}
