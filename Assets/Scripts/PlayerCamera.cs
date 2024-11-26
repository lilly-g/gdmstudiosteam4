using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;

    void Update()
    {
        transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}
