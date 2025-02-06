using UnityEngine;
using System.Collections.Generic;

public class CardHolder : MonoBehaviour
{
    private Stack<Card> cardStack = new Stack<Card>();
    private PlayerController playerController;
    private GrapplingRope grapple;

    public void addCard(Card pCard)
    {
        cardStack.Push(pCard);
    }

    // get cardStack method for UI
    public List<Card> getStackList() {
        return new List<Card>(cardStack);
    }

    public void playCard()
    {
        if (cardStack.Count > 0)
        {
            Card uCard = cardStack.Peek();
            if (uCard != null && uCard.CanPlay(this.gameObject))
            {
                cardStack.Pop();
                uCard.Play(this.gameObject);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            playCard();
        }
    }
}
