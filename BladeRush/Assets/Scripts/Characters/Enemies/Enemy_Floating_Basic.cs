using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Floating_Basic : Enemy {

    public float damage = 10.0f;
    public float attackSpeedMultiplier = 1.0f;
    public float attackCooldown = 2.0f;

    int currAttackStep;
    int maxAttackStep = 10;
    bool bIsAttacking;
    float attackStepTimer = 0.25f;
    float currStepTimer;
    float currCooldown;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        base.Update();

        if (stunTime > 0)
            return;

        if (bHasTarget)
            attack();
	}

    void attack()
    {
        if (currStepTimer > 0) currStepTimer -= Time.deltaTime;
        if (currCooldown > 0) currCooldown -= Time.deltaTime;

        if (bIsAttacking && currStepTimer <= 0 && currAttackStep < maxAttackStep)
        {
            Vector3 vec = transform.rotation * Vector3.forward;
            Vector3 vecdir = player.transform.position - transform.position;
            vec.Normalize();
            vecdir.Normalize();
            fireProjectile(transform.position + (transform.forward * 0.5f), Quaternion.LookRotation(new Vector3(vec.x, vecdir.y, vec.z)));
            currStepTimer = attackStepTimer * attackSpeedMultiplier;
            currAttackStep++;
        }
        else if (!bIsAttacking && currCooldown <= 0)
            startAttack();
        else if (bIsAttacking && currAttackStep >= maxAttackStep)
            resetAttack();
    }

    void startAttack()
    {
        bIsAttacking = true;
        currAttackStep = 0;
        currStepTimer = 0;
    }

    void resetAttack()
    {
        currCooldown = attackCooldown;
        bIsAttacking = false;
    }
}
