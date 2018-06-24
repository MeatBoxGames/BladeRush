using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile {

	// Use this for initialization
	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    protected void OnCollisionEnter(Collision collision)
    {
        PlayerCharacter player = collision.gameObject.GetComponentInParent<PlayerCharacter>();
        // If what we hit was a player.
        if (player != null)
        {
            // The player can't take damage now, but if they could, whatever it is that would cause that should happen here.
            //player.TakeDamage(damage);

            // Instead, we're just going to tell the game we're dead directly.
            // Grab the game controller
            GameMode game = FindObjectOfType<GameMode>();
            Debug.Assert(game != null, "Game is null!");

            // And tell it we're dead.
            if (game != null)
            {
                game.PlayerDied(player);
            }
        }

        Destroy(gameObject);
    }
}
