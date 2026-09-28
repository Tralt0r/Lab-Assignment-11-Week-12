using UnityEngine;

public class enemy : EnemyOrbit
{
    //Takes base meteor logic and applys it to my enemy
    private int hitCount = 0;

    protected override void OnPlayerHit(Collider2D player)
    {
        GameManager.Instance.gameOver = true;
        Destroy(player.gameObject);
    }

    protected override void OnLaserHit(Collider2D laser)
    {
        hitCount++;
    }

    protected override void Update()
    {
        base.Update();

        if (hitCount >= 1)
        {
            if (Player.Instance != null)
            {
                Player.Instance.Shake();
            }
            Destroy(this.gameObject);
        }
    }
}