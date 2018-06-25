using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    public float damage = 10.0f;
    public float attackSpeedMultiplier = 1.0f;
    public int maxAttackStep = 10;
    public float attackCooldown = 2.0f;
    protected float currCooldown;
    public float minWindupTime = 0.0f;
    public float maxWindupTime = 0.0f;

    public float visionRange = 20.0f;
    public float moveSpeed = 7.0f;
    public float rotationSpeed = 1.5f;
    public float maxVertRot = 0.5f;
    protected GameObject player = null;

    public GameObject projectile;

    protected bool bHasTarget = false;

    protected float stunTime;

    int triggerCache;
    int deadTrigger;
    Animator animController;

    protected bool bDead;

	// Use this for initialization
    public void Start() 
    {
        base.Start();
	}
	
	// Update is called once per frame
	public void Update () 
    {
        if (bDead) return;

        if (player == null)
            player = GameObject.FindGameObjectsWithTag("Player")[0];

        if (stunTime > 0) stunTime -= Time.deltaTime;

        if (!bHasTarget)
            findPlayer();
        else
            chasePlayer();
	}

    public void takeDamage(int damage, float stundur)
    {
        if (animController == null)
        {
            animController =  GetComponent<Animator>();
            triggerCache = Animator.StringToHash("TakeDamage");
            deadTrigger = Animator.StringToHash("Die");
        }

        animController.SetTrigger(triggerCache);

        if (!bHasTarget)
        {
            bHasTarget = true;
            currCooldown = Random.Range(minWindupTime, maxWindupTime);
        }

        currHP -= damage;
        stunTime = stundur;

        Debug.Log(currHP);

        if (currHP <= 0)
            die();
    }

    public void die()
    {
        animController.SetTrigger(deadTrigger);
        bDead = true;
        GameMode game = FindObjectOfType<GameMode>();
        game.EnemyDied(this);
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
            currCooldown = Random.Range(minWindupTime, maxWindupTime);
        }
    }

    protected void fireProjectile(Vector3 offset)
    {
        Vector3 vec = transform.rotation * (Vector3.forward + offset);
        Vector3 vecdir = player.transform.position - transform.position;
        vec.Normalize();
        vecdir.Normalize();
        vecdir += offset;
        vecdir.Normalize();
        fireProjectile_Internal(transform.position + (transform.forward * 0.5f), Quaternion.LookRotation(new Vector3(vec.x, vecdir.y, vec.z)));
    }

    void fireProjectile_Internal(Vector3 pos, Quaternion rot)
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
