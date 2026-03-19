using System;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField]
    private Button buttonRef = null;

    private Image imgRef = null;
    private Func<CardView, CardVO> GetCardData = null;

    public void Init(Sprite cardSprite, Func<CardView, CardVO> getCardData)
    {
        imgRef.sprite = cardSprite;
        GetCardData = getCardData;
    }

    public void Start()
    {
        if (buttonRef != null)
        {
            buttonRef.onClick.AddListener(OnCardTap);
        }
    }

    private void SetCard(CardVO card)
    {
        if (card != null)
        {
            imgRef.sprite = card.GetSprite();
        }
    }

    private void OnCardTap ()
    {
        //Flip Card
        CardVO card = GetCardData?.Invoke(this);

        if (card != null)
        {
            SetCard(card);
        }
    }
}