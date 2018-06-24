using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float projectileSpeed;

    protected float damage = 0.0f;
    protected Character characterOwner;
    protected Rigidbody rigidbody;

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
    public void Start() 
    {
        rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
    public void Update()
    {

	}

    protected void OnCollisionEnter(Collision collision)
    {
        
    }
}
