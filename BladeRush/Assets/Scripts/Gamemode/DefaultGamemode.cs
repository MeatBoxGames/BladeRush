using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGamemode : GameMode {
    public AudioSource oVictorySound;

    private enum GameStates
    {
        Playing,
        Victory,
        Dead
    };

    private GameStates state;
    private List<Enemy> oenemies;

	// Use this for initialization
	void Start () {
        // Start us off as playing.
        state = GameStates.Playing;
        oenemies = new List<Enemy>(FindObjectsOfType<Enemy>());
    }
	
	// Update is called once per frame
	void Update () {

    }

    public override void EnemyDied(Enemy deadenemy)
    {
        oenemies.Remove(deadenemy);
        // If all the enemies are dead (i.e. there are no more enemies on the 
        if (oenemies.Count <= 0)
        {
            // The player wins.
            GameVictory();
        }
    }

    void GameVictory()
    {
        // Move to the success state
        state = GameStates.Victory;
        if (oVictorySound != null) {
            oVictorySound.Play();
        }
    }
}
