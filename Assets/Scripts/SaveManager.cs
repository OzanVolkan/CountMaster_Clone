using System.IO;
using UnityEngine;

public class SaveManager
{
    public static void SaveData(GameData gameData)
    {
        var json = JsonUtility.ToJson(gameData);
        File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "GameDataJ.txt", json);
    }

    public static void LoadData(GameData gameData)
    {
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "GameDataJ.txt"))
        {
            var json = File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "GameDataJ.txt");
            JsonUtility.FromJsonOverwrite(json, gameData);
        }
        else
        {
            var json = JsonUtility.ToJson(gameData);
            File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "GameDataJ.txt", json);
        }
    }
}
