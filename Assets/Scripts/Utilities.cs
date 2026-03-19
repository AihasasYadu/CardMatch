using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static (int cols, int rows) CalculateGridDimensions(int totalItems, int maxColsPerRow = 10)
    {
        if (totalItems <= 0) return (0, 0);

        // Ideal square root
        int ideal = Mathf.CeilToInt(Mathf.Sqrt(totalItems));
        
        // Clamp to maxColsPerRow for wide screens
        int cols = Mathf.Clamp(ideal, 1, maxColsPerRow);
        
        // Find best cols that divide totalItems evenly (or minimal waste)
        cols = FindOptimalCols(totalItems, cols, maxColsPerRow);
        
        int rows = (totalItems + cols - 1) / cols; // Ceiling division
        
        return (cols, rows);
    }
    
    private static int FindOptimalCols(int totalItems, int ideal, int maxCols)
    {
        int bestCols = ideal;
        int minWaste = int.MaxValue;
        
        for (int c = Mathf.Max(1, ideal - 2); c <= Mathf.Min(totalItems, maxCols); c++)
        {
            int r = (totalItems + c - 1) / c;
            int waste = Mathf.Abs(c - r) * 10 + (r * c - totalItems); // Prioritize square + fill
            
            if (waste < minWaste)
            {
                minWaste = waste;
                bestCols = c;
            }
        }
        return bestCols;
    }
}