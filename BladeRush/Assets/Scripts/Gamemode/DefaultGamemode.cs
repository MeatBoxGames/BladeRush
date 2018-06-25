using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGamemode : GameMode {
    public AudioSource Victory_Sound;
    public AudioSource Player_Dies_Sound;
    public string Failure_Scene;
    public GameObject Failure_Fadeout;
    public bool God_Mode = false;

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

    public override void PlayerDied(PlayerCharacter player)
    {
        if (!God_Mode) {
            if (state == GameStates.Playing)
            {
                Player_Dies_Sound.Play();
                GameFailure();
            }
        }
    }

    void GameVictory()
    {
        // Move to the success state
        state = GameStates.Victory;
        Time.timeScale = 0;
        if (Victory_Sound != null) {
            Victory_Sound.Play();
        }
    }

    void GameFailure()
    {
        state = GameStates.Dead;
        Instantiate(Failure_Fadeout);
    }
}
