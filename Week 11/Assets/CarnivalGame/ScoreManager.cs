using UnityEngine;
using System.IO;

public class ScoreManager : MonoBehaviour, IHitObserver
{
    public BulletPool pool;
    private int score, hits;

    void OnEnable()  { GameEvents.Hits.Subscribe(this); }
    void OnDisable() { GameEvents.Hits.Unsubscribe(this); }

    //On hit add points
    public void OnTargetHit(TargetHitEvent hitEvent) { score += hitEvent.points; hits++; }
    private string BinPath { get { return System.IO.Path.Combine(Application.persistentDataPath, "score.data"); } }

    //UI add points
    void OnGUI()
    {
        var style = new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold };
        style.normal.textColor = Color.yellow;
        GUI.Label(new Rect(10, 8, 300, 30), "SCORE " + score.ToString("00000"), style);
        GUI.Label(new Rect(10, 38, 300, 30), "HITS " + hits, style);
        if (pool != null) GUI.Label(new Rect(10, 68, 300, 30), "POOL " + pool.InUse + "/" + pool.Total, style);
    }
    //Saves original score
    public void SaveBinary()
    {
        using (BinaryWriter write = new BinaryWriter(File.Open(BinPath, FileMode.Create)))
        {
            write.Write(score);
            write.Write(hits);
        }
    }
    //Reads original score
    public void LoadBinary()
    {
        if (!File.Exists(BinPath)) return;
        using (BinaryReader reader = new BinaryReader(File.Open(BinPath, FileMode.Open)))
        {
            score = reader.ReadInt32();
            hits = reader.ReadInt32();
        }
    }
}
