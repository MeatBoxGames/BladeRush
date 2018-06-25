using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public float maxHP = 100;
    protected float currHP;

	// Use this for initialization
	public void Start () {
        currHP = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
