using UnityEngine;

public class BigMeteor : MeteorBase
{
    //Subclass for meteors
    //# of hits to destroy
    private int hitCount = 0;

    private void Awake()
    {
        fallSpeed = 0.5f;
    }

    //Check hits and if enough shake camera
    protected override void Update()
    {
        base.Update();

        if (hitCount >= 5)
        {
            if (Player.Instance != null)
            {
                Player.Instance.Shake();
            }
            Destroy(this.gameObject);
        }
    }

    //Player hit = game over
    protected override void OnPlayerHit(Collider2D player)
    {
        GameManager.Instance.gameOver = true;
        Destroy(player.gameObject);
    }

    //Hit by laser = destroy and camera shake
    protected override void OnLaserHit(Collider2D laser)
    {
        hitCount++;
        Destroy(laser.gameObject);
    }
}