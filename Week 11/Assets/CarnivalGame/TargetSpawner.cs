using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    private TargetDirector director;
    private float timer;

    void Awake() { director = new TargetDirector(new TargetBuilder()); }
    //Spawn enemies at random
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;
        timer = Random.Range(0.6f, 1.4f);

        int dir = Random.value < 0.5f ? 1 : -1;
        Vector3 pos = new Vector3(-dir * 11f, 0.5f + Random.Range(0, 3) * 1.4f, 0f);
        float range = Random.value;
        if (range < 0.5f) director.Enemy1(pos, dir);
        else if (range < 0.85f) director.Enemy2(pos, dir);
        else director.Enemy3(pos, dir);
    }
}
