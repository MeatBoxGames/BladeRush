using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character {

    public GameObject swordProjectile;
    GameObject swordInstance;
    public int maxStamina;
    float currStamina;
    bool bHasSword = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ThrowSword(Vector3 pos, Quaternion rot)
    {
        if (!bHasSword)
        {
            TeleportSword();
            return;
        }

        swordInstance = (GameObject)Instantiate(
        swordProjectile,
        pos,
        rot);

        swordInstance.GetComponent<Rigidbody>().velocity = swordInstance.transform.forward * swordInstance.GetComponent<Projectile>().projectileSpeed;

        Physics.IgnoreCollision(swordInstance.GetComponent<Collider>(), GetComponent<Collider>());
        swordInstance.GetComponent<Projectile>().setCharacterOwner(GetComponent<Character>());

        bHasSword = false;
    }

    public void TeleportSword()
    {
        transform.position = swordInstance.transform.position;
        Destroy(swordInstance);
        bHasSword = true;

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 9;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up * -1.0f), out hit, 2.0f, layerMask))
        {
            GetComponent<PlayerController_Default>().enableAirPause();
        }
    }
}
