using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    public float visionRange = 20.0f;
    public float moveSpeed = 7.0f;
    public float rotationSpeed = 0.1f;
    public float maxVertRot = 0.5f;
    protected GameObject player = null;

    public GameObject projectile;

    protected bool bHasTarget = false;

    protected float stunTime;

	// Use this for initialization
    public void Start() 
    {

	}
	
	// Update is called once per frame
	public void Update () 
    {
        if (player == null)
            player = GameObject.FindGameObjectsWithTag("Player")[0];

        if (stunTime > 0) stunTime -= Time.deltaTime;

        if (!bHasTarget)
            findPlayer();
        else
            chasePlayer();
	}

    void chasePlayer()
    {
        Vector3 targetDir = player.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotationSpeed * Time.deltaTime, 0.0f);
        if (targetDir.magnitude > 2.0f)
            newDir.y = Mathf.Clamp(newDir.y, maxVertRot * -1.0f, maxVertRot);
        else 
            newDir.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(newDir);

        // If the enemy is stunned, return now so they only rotate and do not move
        if (stunTime > 0) 
            return;
    }

    void findPlayer()
    {
        if (Vector3.Dot(transform.forward, player.transform.position - transform.position) < 0.8f)
            return;

        if (Vector3.Distance(player.transform.position, transform.position) > visionRange)
            return;

                // Bit shift the index of the layer to get a bit mask
        int layermask1 = 1 << 9;
        int layermask2 = 1 << 10;
        int layermask3 = 1 << 11;

        int finalmask = layermask1 | layermask2 | layermask3;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        finalmask = ~finalmask;

        RaycastHit hit;
        if (!Physics.Raycast(transform.position + new Vector3(0.0f, 1.25f, 0.0f), player.transform.position, out hit, 2.0f, finalmask))
        {
            bHasTarget = true;
        }
    }

    protected void fireProjectile(Vector3 pos, Quaternion rot)
    {
        GameObject projectileInstance = (GameObject)Instantiate(
        projectile,
        pos,
        rot);

        projectileInstance.GetComponent<Rigidbody>().velocity = projectileInstance.transform.forward * projectileInstance.GetComponent<Projectile>().projectileSpeed;

        Physics.IgnoreCollision(projectileInstance.GetComponent<Collider>(), GetComponent<Collider>());
        projectileInstance.GetComponent<Projectile>().setCharacterOwner(GetComponent<Character>());
    }

    public void setStunTime(float duration)
    {
        stunTime = duration;
    }
}
