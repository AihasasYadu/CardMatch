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

    public void Init(Sprite cardSprite, bool isMatched, Func<CardView, CardVO> getCardData)
    {
        SetCard(cardSprite);
        GetCardData = getCardData;
        buttonRef.interactable = !isMatched;
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
            Sprite cardSprite = card.GetCardSprite;
            SetCard(cardSprite);
            CardsManager.OnCardsTapped?.Invoke(card);
        }
    }

    public void ResetCard(Sprite cardBackSprite)
    {
        SetCard(cardBackSprite);
        buttonRef.interactable = true;
    }

    public void DisableCard()
    {
        gameObject.SetActive(false);
    }

    public void ShowCard(Transform parent)
    {
        transform.SetParent(parent);
        gameObject.SetActive(true);
    }
}