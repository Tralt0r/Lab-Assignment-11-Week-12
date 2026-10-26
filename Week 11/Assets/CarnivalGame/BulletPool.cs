using System.Collections.Generic;
using UnityEngine;

// ===== OBJECT POOL PATTERN =====
public class BulletPool
{
    private readonly Stack<Bullet> available = new Stack<Bullet>();
    public int Total { get; private set; }
    public int InUse { get { return Total - available.Count; } }

    //Bullet pool save information
    public BulletPool(int size, Transform parent)
    {
        Total = size;
        for (int i = 0; i < size; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "Bullet_" + i;
            go.transform.SetParent(parent);
            go.transform.localScale = new Vector3(0.12f, 0.45f, 0.12f);
            go.GetComponent<Renderer>().material.color = Color.cyan;
            go.GetComponent<Collider>().isTrigger = true;
            go.AddComponent<Rigidbody>().isKinematic = true;
            Bullet bullet = go.AddComponent<Bullet>();
            bullet.Init(this);
            go.SetActive(false);
            available.Push(bullet);
        }
    }
    //Add object to pool
    public Bullet Acquire()
    {
        return available.Count > 0 ? available.Pop() : null;
    }
    //Remove bullet from pool
    public void Release(Bullet bullet)
    {
        if (!bullet.gameObject.activeSelf) return;
        bullet.gameObject.SetActive(false);
        available.Push(bullet);
    }
}
