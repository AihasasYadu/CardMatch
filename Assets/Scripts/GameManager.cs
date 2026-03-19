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
            StartGame(GetLevelReadyCardData());
        }
    }

    private List<CardData> GetLevelReadyCardData()
    {
        // This method can be used to setup card data for the game, 
        // such as shuffling the card data list, creating pairs of matching cards, etc.
        List<CardData> cardDataList = new List<CardData>();

        foreach (CardData cardData in cardDataSOList[levelIndex].GetCardDataList())
        {
            // Create pairs of matching cards by adding the same card data twice to the list
            cardDataList.Add(cardData);
            cardDataList.Add(cardData);
        }

        cardDataList.Shuffle();

        return cardDataList;
    }

    private void StartGame(List<CardData> cardDataList)
    {
        if (cardManager != null)
        {
            int cellsPerSide = Mathf.CeilToInt(Mathf.Sqrt(cardDataList.Count));
            cardManager.InitGrid(cellsPerSide, cardDataList);
        }
    }
}