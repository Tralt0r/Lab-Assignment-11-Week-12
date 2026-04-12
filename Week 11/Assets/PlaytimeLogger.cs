using System.IO;
using UnityEngine;

public class PlaytimeLogger : MonoBehaviour
{
    float sessionStartTime;
    string path;

    void Start()
    {
        sessionStartTime = Time.time;
        path = Application.persistentDataPath + "/playtime.txt";

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Playtime Log\n");
        }
    }

    void OnApplicationQuit()
    {
        float sessionTime = Time.time - sessionStartTime;

        string log = "Session: " + sessionTime + " seconds\n";

        File.AppendAllText(path, log);

        Debug.Log("Playtime saved");
    }
}