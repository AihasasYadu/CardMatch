using System;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField]
    private Button buttonRef = null;

    [SerializeField]
    private Image imgRef = null;
    
    private Func<CardView, CardVO> GetCardData = null;

    public void Init(Sprite cardSprite, Func<CardView, CardVO> getCardData)
    {
        imgRef.sprite = cardSprite;
        GetCardData = getCardData;
    }

    public void OnEnable()
    {
        if (buttonRef != null)
        {
            buttonRef.onClick.AddListener(OnCardTap);
        }
    }

    public void OnDisable()
    {
        if (buttonRef != null)
        {
            buttonRef.onClick.RemoveListener(OnCardTap);
        }
    }

    private void SetCard(Sprite cardSprite)
    {
        if (imgRef != null)
        {
            imgRef.sprite = cardSprite;
        }
    }

    private void OnCardTap ()
    {
        buttonRef.interactable = false;

        //Flip Card
        CardVO card = GetCardData?.Invoke(this);
        if (card != null)
        {
            Sprite cardSprite = card.GetSprite();
            SetCard(cardSprite);
            CardsManager.OnCardsTapped?.Invoke(card);
        }
    }

    public void ResetCard(Sprite cardBackSprite)
    {
        SetCard(cardBackSprite);
        buttonRef.interactable = true;
    }
}