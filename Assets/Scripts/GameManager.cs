using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CardsManager cardManager = null;

    [SerializeField]
    private List<CardDataSO> cardDataSOList = null;

    private int levelIndex = 0;

    public void Start()
    {
        if (cardDataSOList != null && cardDataSOList.Count > 0)
        {
            StartLevel(GetLevelReadyCardData());
        }
    }

    private List<CardVO> GetLevelReadyCardData()
    {
        // This method can be used to setup card data for the game, 
        // such as shuffling the card data list, creating pairs of matching cards, etc.
        List<CardVO> cardsList = new List<CardVO>();

        foreach (CardData cardData in cardDataSOList[levelIndex].CardDataList)
        {
            CardVO cardVO = new CardVO(cardData.matchID, cardData.cardSprite);

            // Create pairs of matching cards by adding the same card data twice to the list
            cardsList.Add(cardVO);
            cardsList.Add(cardVO);
        }

        cardsList.Shuffle();

        return cardsList;
    }

    private void StartLevel(List<CardVO> cardsList)
    {
        if (cardManager != null)
        {
            int cellsPerSide = Mathf.CeilToInt(Mathf.Sqrt(cardsList.Count));
            cardManager.InitGrid(cellsPerSide, cardsList, cardDataSOList[levelIndex].CardThemeSprite);
        }
    }
}