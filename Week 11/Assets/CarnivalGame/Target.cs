using UnityEngine;

public class Target : MonoBehaviour
{
    public float speed = 4f;
    public int points = 10;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (Mathf.Abs(transform.position.x) > 12f) Destroy(gameObject);
    }
}
