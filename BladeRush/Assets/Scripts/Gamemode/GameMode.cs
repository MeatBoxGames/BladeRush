﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void EnemyDied(Enemy deadenemy)
    {

    }

    public virtual void PlayerDied(PlayerCharacter player)
    {

    }
}
