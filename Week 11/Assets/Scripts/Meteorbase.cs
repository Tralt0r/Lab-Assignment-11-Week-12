using UnityEngine;

public abstract class MeteorBase : MonoBehaviour
{
    public float fallSpeed = 2f;

    //Make meteor fall
    protected virtual void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * fallSpeed);

        if (transform.position.y < -11f)
        {
            Destroy(this.gameObject);
        }
    }

    //Check hit box
    private void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if (whatIHit.tag == "Player")
        {
            OnPlayerHit(whatIHit);
        }
        else if (whatIHit.tag == "Laser")
        {
            OnLaserHit(whatIHit);
        }
    }

    //Call these on hit
    protected abstract void OnPlayerHit(Collider2D player);
    protected abstract void OnLaserHit(Collider2D laser);
}