using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 16f;
    private BulletPool pool;

    public void Init(BulletPool bulletPool) { pool = bulletPool; }
    //Fire bullet from position upward
    public void Fire(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        //Return to pool
        if (transform.position.y > 6f) pool.Release(this);
    }

    //Destroy bullet on contact
    void OnTriggerEnter(Collider other)
    {
        if (!gameObject.activeSelf) return;
        Target trans = other.GetComponent<Target>();
        if (trans == null) return;

        //Tell game event to add score
        GameEvents.Hits.Notify(new TargetHitEvent { points = trans.points, position = trans.transform.position });
        Destroy(trans.gameObject);
        pool.Release(this);
    }
}
