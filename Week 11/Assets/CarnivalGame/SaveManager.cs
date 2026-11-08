using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

//Save information
[System.Serializable]
public class SaveEntry
{
    public string saveID;
    public string transformJson;   //transform saver info
    public bool isTarget;
    public float speed;
    public int points;
    public Color color;
}

//All save file objects
[System.Serializable]
public class SaveFile
{
    public List<SaveEntry> entries = new List<SaveEntry>();
}

public class SaveManager : MonoBehaviour
{
    public ScoreManager score;

    //Where the JSON file is saved
    private string JsonPath { get { return Path.Combine(Application.persistentDataPath, "positions.json"); } }

    void Update()
    {
        //Press S and L to save and load
        if (Input.GetKeyDown(KeyCode.S)) Save();
        if (Input.GetKeyDown(KeyCode.L)) Load();
    }

    void Start()
    {
        if (File.Exists(JsonPath)) Load();
    }

    void Save()
    {
        SaveFile file = new SaveFile();
        //Find all objects able to be saved
        var saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();

        foreach (ISaveable save in saveables)
        {
            SaveEntry entry = new SaveEntry { saveID = save.SaveID, transformJson = save.SavedData };
            MonoBehaviour monobehave = (MonoBehaviour)save;

            //If target save its speed, points, and color
            Target target;
            if (monobehave.TryGetComponent(out target))
            {
                entry.isTarget = true;
                entry.speed = target.speed;
                entry.points = target.points;
                entry.color = target.GetComponent<Renderer>().material.color;
            }
            file.entries.Add(entry);
        }

        //Positions go to the JSON file, score goes to the binary file
        File.WriteAllText(JsonPath, JsonUtility.ToJson(file, true));
        score.SaveBinary();
        Debug.LogFormat("Saved to ", Application.persistentDataPath);
    }

    void Load()
    {
        if (!File.Exists(JsonPath)) { Debug.LogWarning("No save file found."); return; }
        SaveFile file = JsonUtility.FromJson<SaveFile>(File.ReadAllText(JsonPath));

        //Find saveable objects by ID
        var lookup = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>()
                     .Where(save => !((MonoBehaviour)save).TryGetComponent<Target>(out _))
                     .ToDictionary(save => save.SaveID, save => save);

        //Remove targets on screen
        foreach (Target target in FindObjectsOfType<Target>()) Destroy(target.gameObject);

        TargetBuilder builder = new TargetBuilder();
        foreach (SaveEntry entry in file.entries)
        {
            if (entry.isTarget)
            {
                //Remake targets and put them in the right positions from before
                Target target = builder.SetSpeed(entry.speed).SetPoints(entry.points).SetColor(entry.color).Build();
                target.GetComponent<TransformSaver>().LoadFromData(entry.transformJson);
            }
            else if (lookup.ContainsKey(entry.saveID))
            {
                //Restore where the shooter was
                lookup[entry.saveID].LoadFromData(entry.transformJson);
            }
        }

        score.LoadBinary();
    }
}