using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    public float visionRange = 20.0f;
    public float moveSpeed = 7.0f;
    public float rotationSpeed = 5.0f;
    public float maxVertRot = 0.5f;
    public GameObject player;

    public bool bHasTarget = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void Update () {
        if (!bHasTarget)
            findPlayer();
        else
            chasePlayer();
	}

    void chasePlayer()
    {
        Vector3 targetDir = player.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotationSpeed * Time.deltaTime, 0.0f);
        newDir.y = Mathf.Clamp(newDir.y, maxVertRot * -1.0f, maxVertRot);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void findPlayer()
    {
        if (Vector3.Dot(transform.forward, player.transform.position - transform.position) < 0.8f)
            return;

        if (Vector3.Distance(player.transform.position, transform.position) > visionRange)
            return;

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 9;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        if (!Physics.Raycast(transform.position + new Vector3(0.0f, 1.25f, 0.0f), player.transform.position, out hit, 2.0f, layerMask))
        {
            bHasTarget = true;
        }
    }
}
