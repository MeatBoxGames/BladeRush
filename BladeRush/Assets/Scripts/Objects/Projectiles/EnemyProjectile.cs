using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile {
    public GameObject Impact_Effect;

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
        // Go through the contact points
        foreach (ContactPoint hitpoint in collision.contacts)
        {
            // Instantiate the impact effect at the impact point.
            GameObject impact = Instantiate(Impact_Effect, hitpoint.point, Quaternion.identity);
            // And tell them to clean themselves up
            Destroy(impact, 1);
        }

        // Get the player that was hit, or null if we hit something else.
        PlayerCharacter player = collision.gameObject.GetComponentInParent<PlayerCharacter>();
        // If what we hit was a player...
        if (player != null)
        {
            // The player can't take damage now, but if they could, whatever it is that would cause that should happen here.
            player.takeDamage(damage);
        }

        Destroy(gameObject);
    }
}
