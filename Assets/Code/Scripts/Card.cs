using UnityEngine;

public enum CardsEnum{
    Grapple,
    Dash
}

public class CardManager
{
    public static Card enumToCard(CardsEnum cardType)
    {
        if (cardType == CardsEnum.Grapple)
        {
            return new GrappleCard();
        }
        else
        {
            return new DashCard();
        }
    }
}

public abstract class Card
{
   abstract public Color getColor();
   abstract public void Play(GameObject pObj);
}

public class GrappleCard : Card
{
    override public Color getColor()
    {
        return Color.blue;
    }

    override public void Play(GameObject pObj)
    {
        Debug.Log("Grapple Card Played!");
        pObj.GetComponentInChildren<GrapplingGun>().SetGrapplePoint();
    }
}

public class DashCard : Card
{
    override public Color getColor()
    {
        return Color.green;
    }

    override public void Play(GameObject pObj)
    {
        Debug.Log("Dash Card Played!");
        pObj.GetComponent<PlayerController>().Dash();
    }
}