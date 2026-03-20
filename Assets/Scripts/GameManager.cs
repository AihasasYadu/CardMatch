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

    private List<CardVO> currentCardData = null;
    private int turnCount = 0;
    private int matchedPairsCount = 0;
    private int currentLevelScore = 0;
    private int previousTurnCount = -1;
    private int previousMatchedPairsCount = -1;
    private int streakCount = 0;
    private int levelIndex = 0;

    public void Start()
    {
        LoadGameData();
        TurnComplete += HandleTurnComplete;
        MatchFound += HandleMatchFound;
        UIManager.PlayButtonTapped += OnPlayButtonTapped;
        UIManager.NextLevelButtonTapped += OnNextLevelButtonTapped;
    }

    private void LoadGameData()
    {
        SaveData savedData = SaveManager.LoadGame();
        if (savedData != null)
        {
            levelIndex = savedData.levelIndex;
            turnCount = savedData.turnCount;
            matchedPairsCount = savedData.matchedPairsCount;
            currentLevelScore = savedData.currentLevelScore;
            currentCardData = savedData.lastLevelCardData;
        }
    }

    private void OnPlayButtonTapped()
    {
        if (cardDataSOList != null && cardDataSOList.Count > 0 && levelIndex < cardDataSOList.Count)
        {
            UIManager.TurnsCounterUpdated?.Invoke(turnCount);
            UIManager.MatchesCounterUpdated?.Invoke(matchedPairsCount);
            UIManager.ScoreCounterUpdated?.Invoke(currentLevelScore);
            GenerateLevelReadyCardData();
            StartLevel(currentCardData);
            cardManager.GenerateGrid();
        }
    }

    private void GenerateLevelReadyCardData()
    {
        if (currentCardData == null)
        {
            // This method can be used to setup card data for the game, 
            // such as shuffling the card data list, creating pairs of matching cards, etc.
            currentCardData = new List<CardVO>();
    
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
                currentCardData.Add(cardVO1);
                currentCardData.Add(cardVO2);
            }
    
            currentCardData.Shuffle();
        }
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
        CheckScore();
        UIManager.MatchesCounterUpdated?.Invoke(matchedPairsCount);
        if (matchedPairsCount == cardDataSOList[levelIndex].CardDataList.Count)
        {
            turnCount = 0;
            matchedPairsCount = 0;
            currentCardData = null;
            OnLevelCompleted?.Invoke();
            // Level complete, move to next level or end game
            levelIndex++;
        }
        SaveGameData();
    }

    private void CheckScore()
    {
        if (turnCount == previousTurnCount + 1 && matchedPairsCount == previousMatchedPairsCount + 1)
        {
            streakCount++;
        }
        else
        {
            streakCount = 0;
        }

        int score = (matchedPairsCount * cardDataSOList[levelIndex].scoreMultiplier) + 
                    (streakCount * cardDataSOList[levelIndex].streakBonusMultiplier);
        currentLevelScore = score;
        previousTurnCount = turnCount;
        previousMatchedPairsCount = matchedPairsCount;
        UIManager.ScoreCounterUpdated?.Invoke(currentLevelScore);
    }

    private void OnNextLevelButtonTapped()
    {
        if (levelIndex < cardDataSOList.Count)
        {
            GenerateLevelReadyCardData();
            StartLevel(currentCardData);
            cardManager.GenerateGrid();
        }
        else
        {
            Debug.Log("Game Completed!");
            OnGameCompleted?.Invoke();
        }
    }

    private void SaveGameData()
    {
        SaveManager.SaveGame(new SaveData
        {
            levelIndex = levelIndex,
            turnCount = turnCount,
            matchedPairsCount = matchedPairsCount,
            currentLevelScore = currentLevelScore,
            lastLevelCardData = currentCardData
        });
    }
}