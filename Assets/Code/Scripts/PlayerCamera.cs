using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;

    void Start()
    {

    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        }

        transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}
