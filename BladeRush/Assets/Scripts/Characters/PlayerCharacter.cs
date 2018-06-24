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

        // Bit shift the index of the layer to get a bit mask
        int layermask1 = 1 << 9;
        int layermask2 = 1 << 10;
        int layermask3 = 1 << 11;

        int finalmask = layermask1 | layermask2 | layermask3;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        finalmask = ~finalmask;

        RaycastHit hit;
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up * -1.0f), out hit, 2.0f, finalmask))
        {
            GetComponent<PlayerController_Default>().enableAirPause();
        }
    }

    public void TeleportSword(Collision collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            Vector3 front = new Vector3(collision.gameObject.transform.forward.x, 0, collision.gameObject.transform.forward.z);
            front.y = 0.0f;
            front.Normalize();
            transform.position = collision.gameObject.transform.position + front * 1.0f + new Vector3(0, 0.5f, 0);
            transform.LookAt(collision.gameObject.transform.position);

            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            Debug.Log("hai");

            if (enemy != null)
                enemy.setStunTime(swordInstance.GetComponent<Sword>().enemyTeleportStunDuration);
        }
        else
        {
            Debug.Log(collision.gameObject.tag);
            transform.position = swordInstance.transform.position;

            ContactPoint contact = collision.contacts[0];

            if (contact.normal.y < 0)
                transform.position -= new Vector3(0, 1.125f, 0);
            else if (contact.normal.y > 0)
                transform.position += new Vector3(0, 1.125f, 0);

            if (contact.normal.y == 0)
                transform.position += contact.normal * 0.25f;
        }

        Destroy(swordInstance);
        bHasSword = true;

        // Bit shift the index of the layer to get a bit mask
        int layermask1 = 1 << 9;
        int layermask2 = 1 << 10;
        int layermask3 = 1 << 11;

        int finalmask = layermask1 | layermask2 | layermask3;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        finalmask = ~finalmask;

        RaycastHit hit;
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up * -1.0f), out hit, 2.0f, finalmask))
        {
            GetComponent<PlayerController_Default>().enableAirPause();
        }
    }
}
