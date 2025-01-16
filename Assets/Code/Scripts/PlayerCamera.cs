using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}
