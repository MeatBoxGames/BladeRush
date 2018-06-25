using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Floating_Shotgun : Enemy {

    int currAttackStep;
    int maxAttackStep = 1;
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
            for (int i = 0; i < 10; i++)
            {
                Vector3 offset = RandomInCone(50);
                //offset += Random.Range(0, 0.5f) * transform.forward;
                fireProjectile(offset);
            }
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

    public static Vector3 RandomInCone(float radius)
    {
        float radradius = radius * Mathf.PI / 360;
        float z = Random.Range(Mathf.Cos(radradius), 1);
        float t = Random.Range(0, Mathf.PI * 2);
        return new Vector3(Mathf.Sqrt(1 - z * z) * Mathf.Cos(t), Mathf.Sqrt(1 - z * z) * Mathf.Sin(t), z);
    }
}
