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

    virtual public bool CanPlay(GameObject pObj)
    {
        if (pObj.GetComponentInChildren<GrapplingRope>().isGrappling || pObj.GetComponent<PlayerController>().isDashing)
        {
            return false;
        }
        else{
            return true;
        }
    }
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
        pObj.GetComponentInChildren<GrapplingGun>().SetGrapple();
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

    override public bool CanPlay(GameObject pObj)
    {
        if (!base.CanPlay(pObj) || new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) == Vector2.zero)
        {
            return false;
        }
        else{
            return true;
        }
    }
}