using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public int coins;
    public int score;

    string path;

    void Awake()
    {
        path = Application.persistentDataPath + "/save.json";
        LoadGame();
    }

    public void SaveGame()
    {
        GameData data = new GameData
        {
            coins = coins,
            score = score
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameData data = JsonUtility.FromJson<GameData>(json);

            coins = data.coins;
            score = data.score;

            Debug.Log("Game Loaded");
        }
    }
}