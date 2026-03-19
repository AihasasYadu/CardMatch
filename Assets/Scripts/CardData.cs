using UnityEngine;

[System.Serializable]
public class CardData
{
    public int matchID = -1;
    public Sprite cardSprite = null;

    public CardData(int id, Sprite sprite)
    {
        matchID = id;
        cardSprite = sprite;
    }
}