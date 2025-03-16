using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCard : MonoBehaviour
{
    private GameObject playerWithCardHolder;

    private CardHolder cardHolder;

    [SerializeField] private Sprite grappleSprite;
    [SerializeField] private Sprite dashSprite;

    [SerializeField] private Image currCard;
    [SerializeField] private Image nextCard;

    void Start()
    {
        //For me sometimes the game doesnt run due to NullReferenceException,
        //It could be because the player take some time to generate at the start of game
        //Subsituting with the commented out code below solved my problem
        playerWithCardHolder = GameObject.FindWithTag("Player");
        cardHolder = playerWithCardHolder.GetComponent<CardHolder>();

        /*      
        playerWithCardHolder = GameObject.FindWithTag("Player");

        if (playerWithCardHolder != null) {
            cardHolder = playerWithCardHolder.GetComponent<CardHolder>();
        }
        else{ 
            Debug.Log("Looking for Player object...");
        }
        */     
    }

    void Update() 
    {
        if (playerWithCardHolder == null)
        {
            playerWithCardHolder = GameObject.FindWithTag("Player");
            cardHolder = playerWithCardHolder.GetComponent<CardHolder>();
        }

        /*
        while (playerWithCardHolder == null)
        {
            playerWithCardHolder = GameObject.FindWithTag("Player");
            cardHolder = playerWithCardHolder.GetComponent<CardHolder>();
        }
        */

        List<Card> cardList = cardHolder.getStackList();

        switch (cardList.Count) 
        {
            case 0:
                currCard.sprite = null;
                nextCard.sprite = null;
                break;
            case 1:
                currCard.sprite = getSpritebyCard(cardList[0]);
                nextCard.sprite = null;
                break;
            default:
                currCard.sprite = getSpritebyCard(cardList[0]);
                nextCard.sprite = getSpritebyCard(cardList[1]);
                break;
        }
    }

    Sprite getSpritebyCard(Card aCard)
    {
        if (aCard != null)
        {
            if (aCard.GetType() == typeof(GrappleCard))
            {
                return grappleSprite;
            }
            else if (aCard.GetType() == typeof(DashCard))
            {
                return dashSprite;
            }
        }
        return null;
    }
}
