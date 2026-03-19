using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataSO", menuName = "ScriptableObjects/CardDataSO", order = 1)]
public class CardDataSO : ScriptableObject
{
    public List<CardData> cardDataList = new List<CardData>();

    public List<CardData> GetCardDataList()
    {
        return cardDataList;
    }

    public void OnValidate()
    {
        if (cardDataList != null)
        {
            if (cardDataList.Select(x => x.matchID).Distinct().Count() != cardDataList.Count)
            {
                Debug.LogError("CardDataSO: Duplicate card data found in the list.");
            }
        }
    }
}