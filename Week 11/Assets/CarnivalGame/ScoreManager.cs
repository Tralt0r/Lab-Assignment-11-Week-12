using UnityEngine;

public class ScoreManager : MonoBehaviour, IHitObserver
{
    public BulletPool pool;
    private int score, hits;

    void OnEnable()  { GameEvents.Hits.Subscribe(this); }
    void OnDisable() { GameEvents.Hits.Unsubscribe(this); }

    //On hit add points
    public void OnTargetHit(TargetHitEvent hitEvent) { score += hitEvent.points; hits++; }

    //UI add points
    void OnGUI()
    {
        var style = new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold };
        style.normal.textColor = Color.yellow;
        GUI.Label(new Rect(10, 8, 300, 30), "SCORE " + score.ToString("00000"), style);
        GUI.Label(new Rect(10, 38, 300, 30), "HITS " + hits, style);
        if (pool != null) GUI.Label(new Rect(10, 68, 300, 30), "POOL " + pool.InUse + "/" + pool.Total, style);
    }
}
