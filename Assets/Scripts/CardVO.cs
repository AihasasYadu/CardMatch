using UnityEngine;

public class CardVO
{
    private int? cardID = null;
    private int matchID = -1;
    private Sprite cardSprite = null;
    private bool isMatched = false;

    public CardVO(int cId, int mId, Sprite sprite)
    {
        cardID = cId;
        matchID = mId;
        cardSprite = sprite;
    }

    public Sprite GetSprite()
    {
        return cardSprite;
    }

    public int GetMatchID()
    {
        return matchID;
    }

    public int GetCardID()
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
