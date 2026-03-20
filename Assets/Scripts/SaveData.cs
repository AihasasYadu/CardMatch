using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[Serializable]
public class SaveData
{
    public int levelIndex;
    public int turnCount;
    public int matchedPairsCount;
    public int currentLevelScore;
    
    [Serialize]
    public List<CardVO> lastLevelCardData;
}