using UnityEngine;

public class CardVO
{
    private int? cardID = null;
    private Sprite cardSprite = null;
    private bool isMatched = false;

    public CardVO(int id, Sprite sprite)
    {
        cardID = id;
        cardSprite = sprite;
    }

    public Sprite GetSprite()
    {
        return cardSprite;
    }

    public int GetID()
    {
        return cardID.Value;
    }

    public void MatchCard()
    {
        isMatched = true;
    }

    public bool IsMatched()
    {
        return isMatched;
    }
}
