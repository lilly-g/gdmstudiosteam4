using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D player;
    public float speed = 10;
    public float jump = 8;
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;
    public bool pulling = false;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!pulling)
        {
            player.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, player.linearVelocity.y);

            if (Input.GetKey(KeyCode.Space) && isGrounded())
            {
                player.linearVelocity = new Vector2(player.linearVelocity.x, jump);
            }
        }
    }

    public void pullPlayer(Vector3 point)
    {
        pulling = true;
        Vector3 direction = (point - transform.position).normalized * 50;
        player.linearVelocity = new Vector2(direction.x, direction.y);
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }

}
