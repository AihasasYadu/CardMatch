using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

public class CardsManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField]
    public CardView cardPrefab;

    [SerializeField]
    public int cellsPerSide = 4; // Grid resolution

    [SerializeField]
    public float padding = 0.02f;

    private RectTransform parentRect;
    private GridLayoutGroup gridLayout;
    private List<CardData> cardDataList = new List<CardData>();
    private Dictionary<CardVO, CardView> cardMap = new Dictionary<CardVO, CardView>();

    void Start()
    {
        parentRect = GetComponent<RectTransform>();
        gridLayout = GetComponent<GridLayoutGroup>();
        GenerateGrid();
    }

    internal void InitGrid(int cellsPerSide, List<CardData> cardDataList)
    {
        this.cellsPerSide = cellsPerSide;
        this.cardDataList = cardDataList;
    }

    public void GenerateGrid()
    {
        if (cellsPerSide > 0 && cardPrefab != null && parentRect != null)
        {
            Vector2 parentSize = parentRect.rect.size;
            float cellSize = Mathf.Min(parentSize.x, parentSize.y) / cellsPerSide;
            int cols = Mathf.Max(cellsPerSide, Mathf.FloorToInt(parentSize.x / cellSize));
            int rows = Mathf.Max(cellsPerSide, Mathf.FloorToInt(parentSize.y / cellSize));

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
                    CardView cell = Instantiate(cardPrefab, transform);
                    cell.name = $"Card_{row}_{col}";
                    CardVO vo = new CardVO(index, cardDataList[index].cardSprite);
                    cardMap.Add(vo, cell);
                    index++;
                }
            }
        }
    }

    void ClearGrid()
    {
        foreach (CardView cell in cardMap.Values)
        {
            if (cell != null) DestroyImmediate(cell);
        }
        cardMap.Clear();
    }
}