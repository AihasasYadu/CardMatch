using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

public class CardsManager : MonoBehaviour
{
    public static Action<CardVO> OnCardsTapped = null;

    [Header("Grid Settings")]
    [SerializeField]
    public CardView cardPrefab;

    [SerializeField]
    public int cellsPerSide = 4; // Grid resolution

    [SerializeField]
    public float padding = 0.02f;

    [SerializeField] 
    private PoolManager<CardView> cardPool = null;

    private RectTransform parentRect;
    private GridLayoutGroup gridLayout;
    private Sprite cardBackSprite = null;
    private List<CardVO> cardDataList = new List<CardVO>();
    private Dictionary<CardVO, CardView> cardMap = new Dictionary<CardVO, CardView>();
    private Tuple<CardVO, CardVO> flippedCards = new Tuple<CardVO, CardVO>(null, null);

    void Awake()
    {
        parentRect = GetComponent<RectTransform>();
        gridLayout = GetComponent<GridLayoutGroup>();
        OnCardsTapped += HandleCardsTapped;
        GameManager.OnLevelCompleted += ClearGrid;
        cardPool = new PoolManager<CardView>(
            createFunc: () => Instantiate(cardPrefab),
            onGet: OnCardGet,
            onRelease: OnCardRelease,
            onDestroy: OnCardDestroy,
            collectionCheck: false,
            defaultCapacity: 20,
            maxSize: 100
        );
    }

    private void OnCardDestroy(CardView card)
    {
        Destroy(card.gameObject);
    }

    private void OnCardRelease(CardView card)
    {
        card.ResetCard(cardBackSprite);
        card.DisableCard();
    }

    private void OnCardGet(CardView card)
    {
        card.ShowCard(this.transform);
    }

    internal void InitGrid(int cellsPerSide, List<CardVO> cardDataList, Sprite cardBackSprite)
    {
        this.cellsPerSide = cellsPerSide;
        this.cardDataList = cardDataList;
        this.cardBackSprite = cardBackSprite;
    }

    public void GenerateGrid()
    {
        if (cellsPerSide > 0 && cardPrefab != null && parentRect != null)
        {
            Vector2 parentSize = parentRect.rect.size;
            int totalCards = cardDataList.Count;
            float cellSize = Mathf.Min(parentSize.x, parentSize.y) / cellsPerSide;
            int rows, cols = 0;
            (rows, cols) = Utilities.CalculateGridDimensions(totalCards, cellsPerSide);

            gridLayout.cellSize = Vector2.one * cellSize;
            gridLayout.spacing = Vector2.one * (cellSize * padding);

            gridLayout.constraintCount = cols; // Fixed cols, auto rows

            // using same index for traversing through card data list and
            // to assign unique ID to each card VO. As the card data list already 
            // be shuffled and contains pairs of matching cards from Game Manager
            int index = 0;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    CardView card = cardPool.Get();
                    card.name = $"Card_{row}_{col}";
                    card.Init(cardBackSprite, GetCardData);
                    cardMap.Add(cardDataList[index], card);
                    index++;
                }
            }
        }
    }

    private CardVO GetCardData(CardView view)
    {
        CardVO cardData = null;
        if (cardMap != null && cardMap.ContainsValue(view))
        {
            cardData = cardMap.First(kvp => kvp.Value == view).Key;
        }
        return cardData;
    }

    private void HandleCardsTapped(CardVO card)
    {
        if (card != null)
        {
            if (flippedCards.Item1 == null)
            {
                flippedCards = new Tuple<CardVO, CardVO>(card, null);
            }
            else if (flippedCards.Item2 == null)
            {
                flippedCards = new Tuple<CardVO, CardVO>(flippedCards.Item1, card);
                GameManager.TurnComplete?.Invoke();
                if (CheckForMatch())
                {
                    UpdateCardStatus();
                    ResetFlippedCards();
                    GameManager.MatchFound?.Invoke();
                }
                else
                {
                    // No match, flip back cards after a short delay
                    StartCoroutine(FlipBackCards());
                    ResetFlippedCards();
                }
            }
        }
    }

    private bool CheckForMatch()
    {
        return flippedCards.Item1.GetMatchID() == flippedCards.Item2.GetMatchID();
    }

    private void UpdateCardStatus()
    {
        flippedCards.Item1.MatchCard();
        flippedCards.Item2.MatchCard();
    }

    private IEnumerator FlipBackCards()
    {
        if (cardMap != null)
        {
            CardVO temp1 = flippedCards.Item1;
            CardVO temp2 = flippedCards.Item2;
            yield return new WaitForSeconds(1f);
            
            if (temp1 != null)
            {
                CardView cardView1 = cardMap[temp1];
                cardView1.ResetCard(cardBackSprite);
            }

            if (temp2 != null)
            {
                CardView cardView2 = cardMap[temp2];
                cardView2.ResetCard(cardBackSprite);
            }
        }
    }

    private void ResetFlippedCards()
    {
        flippedCards = new Tuple<CardVO, CardVO>(null, null);
    }

    private void ClearGrid()
    {
        foreach (CardView card in cardMap.Values)
        {
            if (card != null)
            {
                cardPool.Return(card);
            }
        }
        cardMap.Clear();
        flippedCards = new Tuple<CardVO, CardVO>(null, null);
    }
}