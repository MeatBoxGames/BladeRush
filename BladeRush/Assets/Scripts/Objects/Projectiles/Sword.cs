using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Projectile
{
    public float enemyTeleportStunDuration = 2.0f;
    public bool bHit = false;
    private Quaternion q;
    private Vector3 v3;

    // Use this for initialization
    void Start()
    {
        base.Start();
        Physics.IgnoreLayerCollision(10, 9, false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (bHit)
        {
            transform.position = v3;
            transform.rotation = q;
        }
        else
        {
            v3 = transform.position;
            q = transform.rotation;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        bHit = true;
        rigidbody.velocity = Vector3.zero;
        rigidbody.isKinematic = true;
        PlayerCharacter player = (PlayerCharacter)characterOwner;

        player.TeleportSword(collision);

        base.OnCollisionEnter(collision);
    }

}
