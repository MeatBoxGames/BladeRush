using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameWiggle : MonoBehaviour {

    public float Wiggle_Limit=0.5f;
    public float Wiggle_Amount=0.05f;

    private Vector3 ocurrentoffset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 shift = new Vector3(Random.Range(-Wiggle_Amount, Wiggle_Amount), Random.Range(-Wiggle_Amount, Wiggle_Amount), Random.Range(-Wiggle_Amount, Wiggle_Amount));
        ocurrentoffset += shift;
        ocurrentoffset = Vector3.ClampMagnitude(ocurrentoffset, Wiggle_Limit);
        gameObject.transform.localPosition=ocurrentoffset;
    }
}
