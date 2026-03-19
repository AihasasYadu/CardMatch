using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataSO", menuName = "ScriptableObjects/CardDataSO", order = 1)]
public class CardDataSO : ScriptableObject
{
    public Sprite CardThemeSprite = null;
    public List<CardData> CardDataList = new List<CardData>();

    public void OnValidate()
    {
        if (CardDataList != null)
        {
            if (CardDataList.Select(x => x.matchID).Distinct().Count() != CardDataList.Count)
            {
                Debug.LogError("CardDataSO: Duplicate card data found in the list.");
            }
        }
    }
}