using UnityEngine;
using System.Collections;

[ExecuteAlways]
public class CardCollectable : MonoBehaviour
{
    [SerializeField] private CardsEnum card;
    [SerializeField] private bool canRespawn;
    private SpriteRenderer spriteRenderer;
    private bool consumed = false;
    private float respawnTime = 1f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = CardManager.enumToCard(card).getColor();
    }

    void Update()
    {
        //for the sake of convenience the colour of the sprite will automatically update in the editor if changed
        Color cardColor = CardManager.enumToCard(card).getColor();
        if (spriteRenderer.color != cardColor)
        {
            spriteRenderer.color = cardColor;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //player has multiple colliders, checks if collider is capsule to ensure collectable only interacts with one player collider
        if (!consumed && other.gameObject.CompareTag("Player") && other.GetType().ToString().Equals("UnityEngine.CapsuleCollider2D"))
        {
            Card cardObj = CardManager.enumToCard(card);
            cardObj.setSource(this);

            CardHolder holder = other.gameObject.GetComponent<CardHolder>();
            holder.addCard(cardObj);

            if (!canRespawn)
            {
                gameObject.SetActive(false);
            }
            else{
                consumed = true;
                spriteRenderer.enabled = false;
            }
        }
    }

    public void SignalCardPlayed()
    {
        if (canRespawn)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        consumed = false;
        spriteRenderer.enabled = true;
    }
}
