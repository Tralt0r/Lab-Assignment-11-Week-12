using UnityEngine;

[System.Serializable]
//Object position and scale on screen
public class TransformData
{
    public Vector3 position;
    public Vector3 scale;
}

public class TransformSaver : MonoBehaviour, ISaveable
{
    private string saveID;
    //Assigns each object a int ID to save
    public string SaveID
    {
        get { if (string.IsNullOrEmpty(saveID)) saveID = System.Guid.NewGuid().ToString(); return saveID; }
        set { saveID = value; }
    }

    //Makes current position and scale into a string
    public string SavedData
    {
        get { return JsonUtility.ToJson(new TransformData { position = transform.position, scale = transform.localScale }); }
    }

    //Gets the JSON save stuff and puts the original object back on screen to its original position
    public void LoadFromData(string json)
    {
        TransformData data = JsonUtility.FromJson<TransformData>(json);
        transform.position = data.position;
        transform.localScale = data.scale;
    }
}