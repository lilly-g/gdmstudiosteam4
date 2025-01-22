using UnityEngine;
using System.Collections.Generic;

public class CardHolder : MonoBehaviour
{
    private Stack<Card> cardStack = new Stack<Card>();

    public void addCard(Card pCard)
    {
        cardStack.Push(pCard);
    }

    public void playCard()
    {
        if (cardStack.Count > 0)
        {
            Card uCard = cardStack.Pop();
            if (uCard != null)
            {
                uCard.Play(this.gameObject);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            playCard();
        }
    }
}
