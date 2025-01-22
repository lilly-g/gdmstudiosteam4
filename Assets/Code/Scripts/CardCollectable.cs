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
        if (other.gameObject.CompareTag("Player"))
        {
            CardHolder holder = other.gameObject.GetComponent<CardHolder>();
            holder.addCard(CardManager.enumToCard(card));
            this.gameObject.SetActive(false);
        }
    }
}
