using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float verticalOffset = 4f;
    [SerializeField] private float horizontalOffset = 2f;
    private Rigidbody2D player;
    private float currentHorizontalOffset;

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        }

        if (player.linearVelocity.x < -1 || player.linearVelocity.x > 1)
        {
            currentHorizontalOffset = Mathf.Lerp(currentHorizontalOffset, ((player.linearVelocity.x > 0) ? horizontalOffset : -horizontalOffset), moveSpeed * Time.deltaTime);
        }
        else{
            currentHorizontalOffset = Mathf.Lerp(currentHorizontalOffset, 0f, moveSpeed * Time.deltaTime);
        }

        Vector3 targetPosition = new Vector3 (player.transform.position.x + currentHorizontalOffset, player.transform.position.y + verticalOffset, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
