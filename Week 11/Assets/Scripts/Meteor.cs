using UnityEngine;

public class Meteor : MeteorBase
{
    //Subclass for meteors
    private void Awake()
    {
        fallSpeed = 2f;
    }

    //Player hit = game over
    protected override void OnPlayerHit(Collider2D player)
    {
        GameManager.Instance.gameOver = true;
        Destroy(player.gameObject);
        Destroy(this.gameObject);
    }

    //Hit by laser = destroy and camera shake
    protected override void OnLaserHit(Collider2D laser)
    {
        GameManager.Instance.meteorCount++;
        Player.Instance.Shake();
        Destroy(laser.gameObject);
        Destroy(this.gameObject);
    }
}