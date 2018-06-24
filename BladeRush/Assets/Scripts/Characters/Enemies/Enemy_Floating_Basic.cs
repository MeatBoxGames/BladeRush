using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Floating_Basic : Enemy {

    public float damage = 10.0f;
    public float attackSpeedMultiplier = 1.0f;
    public float attackCooldown = 2.0f;
    public GameObject projectile;

    int currAttackStep;
    int maxAttackStep = 10;
    bool bIsAttacking;
    float attackStepTimer = 0.1f;
    float currStepTimer;
    float currCooldown;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();

        if (bHasTarget)
            attack();
	}

    void attack()
    {
        if (currStepTimer > 0) currStepTimer -= Time.deltaTime;
        if (currCooldown > 0) currCooldown -= Time.deltaTime;

        if (bIsAttacking && currStepTimer <= 0 && currAttackStep < maxAttackStep)
        {
            fireProjectile(transform.position + (transform.forward * 0.5f), transform.rotation);
            currStepTimer = attackStepTimer * attackSpeedMultiplier;
            maxAttackStep++;
        }
        else if (!bIsAttacking && currCooldown <= 0 || currAttackStep >= maxAttackStep)
        {
            currStepTimer = attackStepTimer;
            currCooldown = attackCooldown;
            bIsAttacking = true;
            currAttackStep = 0;
        }
    }

    void resetAttack()
    {
        currCooldown = attackCooldown;
        bIsAttacking = false;
    }

    void fireProjectile(Vector3 pos, Quaternion rot)
    {
        GameObject projectileInstance = (GameObject)Instantiate(
        projectile,
        pos,
        rot);

        projectileInstance.GetComponent<Rigidbody>().velocity = projectileInstance.transform.forward * projectileInstance.GetComponent<Projectile>().projectileSpeed;

        Physics.IgnoreCollision(projectileInstance.GetComponent<Collider>(), GetComponent<Collider>());
        projectileInstance.GetComponent<Projectile>().setCharacterOwner(GetComponent<Character>());
    }
}
