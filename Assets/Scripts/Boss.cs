using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This script is for generic boss actions and value,  and should be attached to every boss 

public class Boss : MonoBehaviour {
    LevelManager level;
    UI ui;
    BossHealth bar;
    PlayerManager player;
    GameManager game;

    public int attack, health, maxHealth;
    public bool bossDead = false;

	// Use this for initialization
	void Start ()
    {
        game = GameManager.instance;
        player = PlayerManager.instance;
        level = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        ui = UI.instance;
        bar = GameObject.Find("Boss Health").GetComponent<BossHealth>();
        if (level==null)
        {
            Debug.LogError("Unable to locate level manager");
        }
        //activate mods
        attack += game.enemyAttackMod;
        maxHealth += game.enemyHealthMod;
        health = maxHealth;

        //add boss to enemy count
        level.enemyCount += 1;
        //link the boss to the healthbar
        print("Attempting to link " + this.gameObject.name + " to the boss healthbar");
        bar.LinkBoss(this.gameObject);
        //set visiblity of the boss healthbar
        ui.BossHealthVisibility(true);

    
	}
	
	// Update is called once per frame
	void Update ()
    {
        //check if boss is dead
        if (health <= 0&&bossDead==false)
        {
            bossDead = true;
            BossCleanup();
        }
		
	}

    public void DamageBossFromPlayer()
    {
        health -= player.attack;
    }


    //generic boss cleanup
    public void BossCleanup()
    {
        gameObject.GetComponent<Animator>().SetBool("isDead", true);
        level.enemyCount -= 1;
        ui.BossHealthVisibility(false);
        //beat the boss kill all the minions
        level.KillAllEnemies();
        //beat a boss, get your rewards
        game.ApplyMods();
        //beat a boss. get a nice chunk of health back
        player.Healing(35);
    }
}
