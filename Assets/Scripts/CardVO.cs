using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class CardVO
{
    [SerializeField]
    private int? cardID = null;
    public int GetCardID => cardID.Value;

    [SerializeField]
    private int matchID = -1;
    public int GetMatchID => matchID;

    [SerializeField]
    private Sprite cardSprite = null;
    public Sprite GetCardSprite => cardSprite;

    [SerializeField]
    private bool isMatched = false;
    public bool IsMatched { get { return isMatched; } set { isMatched = value; } }

    public CardVO(int cId, int mId, Sprite sprite)
    {
        cardID = cId;
        matchID = mId;
        cardSprite = sprite;
    }
}
