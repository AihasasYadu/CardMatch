using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static string saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");

    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game saved to " + saveFilePath);
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded from " + saveFilePath);
            return data;
        }
        else
        {
            Debug.Log("No save file found, returning default data");
            return new SaveData();
        }
    }
}