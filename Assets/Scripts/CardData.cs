using UnityEngine;

[System.Serializable]
public class CardData
{
    public int cardID = -1;
    public Sprite cardSprite = null;

    public CardData(int id, Sprite sprite)
    {
        cardID = id;
        cardSprite = sprite;
    }
}