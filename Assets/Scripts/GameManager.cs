using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action TurnComplete = null;
    public static Action MatchFound = null;
    public static Action OnLevelCompleted = null;
    public static Action OnGameCompleted = null;

    [SerializeField]
    private CardsManager cardManager = null;

    [SerializeField]
    private List<CardDataSO> cardDataSOList = null;

    private int turnCount = 0;
    private int matchedPairsCount = 0;
    private int levelIndex = 0;

    public void Start()
    {
        TurnComplete += HandleTurnComplete;
        MatchFound += HandleMatchFound;
        UIManager.PlayButtonTapped += OnPlayButtonTapped;
        UIManager.NextLevelButtonTapped += OnNextLevelButtonTapped;
    }

    private void OnPlayButtonTapped()
    {
        if (cardDataSOList != null && cardDataSOList.Count > 0)
        {
            StartLevel(GetLevelReadyCardData());
            cardManager.GenerateGrid();
        }
    }

    private List<CardVO> GetLevelReadyCardData()
    {
        // This method can be used to setup card data for the game, 
        // such as shuffling the card data list, creating pairs of matching cards, etc.
        List<CardVO> cardsList = new List<CardVO>();

        // assigning incremental index as unique IDs
        int id = 1;
        foreach (CardData cardData in cardDataSOList[levelIndex].CardDataList)
        {
            // Increment ID for the next card
            CardVO cardVO1 = new CardVO(id, cardData.matchID, cardData.cardSprite);
            id++;
            
            CardVO cardVO2 = new CardVO(id, cardData.matchID, cardData.cardSprite);
            id++;

            // Create pairs of matching cards by adding the same card data twice to the list
            cardsList.Add(cardVO1);
            cardsList.Add(cardVO2);
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

    private void HandleTurnComplete()
    {
        turnCount++;
        UIManager.TurnsCounterUpdated?.Invoke(turnCount);
    }

    private void HandleMatchFound()
    {
        matchedPairsCount++;
        UIManager.MatchesCounterUpdated?.Invoke(matchedPairsCount);
        if (matchedPairsCount == cardDataSOList[levelIndex].CardDataList.Count)
        {
            turnCount = 0;
            matchedPairsCount = 0;
            OnLevelCompleted?.Invoke();
            // Level complete, move to next level or end game
            levelIndex++;
        }
    }

    private void OnNextLevelButtonTapped()
    {
        if (levelIndex < cardDataSOList.Count)
        {
            StartLevel(GetLevelReadyCardData());
            cardManager.GenerateGrid();
        }
        else
        {
            Debug.Log("Game Completed!");
            OnGameCompleted?.Invoke();
        }
    }
}