using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Floating_Basic : Enemy {

    int currAttackStep;
    bool bIsAttacking;
    float attackStepTimer = 0.25f;
    float currStepTimer;

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
            Vector3 offset = new Vector3();
            fireProjectile(offset);
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
