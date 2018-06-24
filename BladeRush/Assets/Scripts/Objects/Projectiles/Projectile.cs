using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float projectileSpeed;
    Character characterOwner;
    Rigidbody rigidbody;
    bool bHit = false;

    private Quaternion q;
    private Vector3 v3;

    public Character getCharacterOwner()
    {
        return characterOwner;
    }

    public void setCharacterOwner(Character owner)
    {
        characterOwner = owner;
        rigidbody = GetComponent<Rigidbody>();
    }

	// Use this for initialization
	void Start () 
    {
        rigidbody = GetComponent<Rigidbody>();
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
        player.TeleportSword();
    }
}
