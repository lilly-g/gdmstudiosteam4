using UnityEngine;

public class CardCollectable : MonoBehaviour
{
    [SerializeField] private CardsEnum card;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = CardManager.enumToCard(card).getColor();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //player has multiple colliders, checks if collider is capsule to ensure collectable only interacts with one player collider
        if (other.gameObject.CompareTag("Player") && other.GetType().ToString().Equals("UnityEngine.CapsuleCollider2D"))
        {
            CardHolder holder = other.gameObject.GetComponent<CardHolder>();
            holder.addCard(CardManager.enumToCard(card));
            this.gameObject.SetActive(false);
        }
    }
}
