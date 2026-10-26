using UnityEngine;

public class Shooter : MonoBehaviour
{
    public float moveSpeed = 8f, fireCooldown = 0.1f;
    private BulletPool pool;
    private float cooldown;

    public void Init(BulletPool bulletPool) { pool = bulletPool; }

    void Update()
    {
        //Move side to side based on player input
        float x = transform.position.x + Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(Mathf.Clamp(x, -8f, 8f), transform.position.y, 0f);

        //If shoot held down add bullet to pool
        cooldown -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && cooldown <= 0f)
        {
            Bullet bullet = pool.Acquire();
            if (bullet != null) { bullet.Fire(transform.position + Vector3.up * 0.5f); cooldown = fireCooldown; }
        }
    }
}
