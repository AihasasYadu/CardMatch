using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
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
    private List<CardView> cardsList = new List<CardView>();

    void Start()
    {
        parentRect = GetComponent<RectTransform>();
        gridLayout = GetComponent<GridLayoutGroup>();
        GenerateGrid();
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

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    CardView cell = Instantiate(cardPrefab, transform);
                    cell.name = $"Cell_{row}_{col}";
                    // Optional: Add custom logic here, e.g., cell.GetComponent<Image>().color = GetCellColor(row, col);
                    cardsList.Add(cell);
                }
            }
        }
    }

    void ClearGrid()
    {
        foreach (CardView cell in cardsList)
        {
            if (cell != null) DestroyImmediate(cell);
        }
        cardsList.Clear();
    }
}
