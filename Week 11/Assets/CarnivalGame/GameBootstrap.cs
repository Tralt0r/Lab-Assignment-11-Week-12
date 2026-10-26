using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    void Awake() { GameEvents.Reset(); }

    void Start()
    {
        //Use current camera
        Camera cam = Camera.main;
        cam.orthographic = true; cam.orthographicSize = 5f;
        cam.transform.position = new Vector3(0, 0, -10);
        cam.clearFlags = CameraClearFlags.SolidColor;

        //Limit bullet pool
        BulletPool pool = new BulletPool(16, transform);

        //Add mini game components
        GameObject shooter = GameObject.CreatePrimitive(PrimitiveType.Cube);
        shooter.name = "Shooter";
        shooter.transform.position = new Vector3(0, -4.2f, 0);
        shooter.transform.localScale = new Vector3(1.2f, 0.5f, 1f);
        shooter.GetComponent<Renderer>().material.color = Color.yellow;
        shooter.AddComponent<Shooter>().Init(pool);
        gameObject.AddComponent<ScoreManager>().pool = pool;
        gameObject.AddComponent<TargetSpawner>();
    }
}
