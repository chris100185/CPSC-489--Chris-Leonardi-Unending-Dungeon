using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//level mods:   At the end of every level, several modifications will be applied. The number and quality of these can be modified but at game start
//              the default is 1 positive mod, and one negative mod. this script will handle to logic for selection of and implementation of the mods
public class LevelMods : MonoBehaviour {
    GameManager game;   //This script only handles selection and implementation of the mods for organization reasons, the actual data for implemented mods is kept
                        //on the manager script.
    PlayerManager player;
    UI ui;

    int enemyHealthIncrease=0;
    int enemyAttackIncrease = 0;

    int playerHealthIncrease = 0;
    //int playerAttackIncrease = 0;

    bool boneAttack = false;

    int negCount = 4;
    int posCount = 2;

    string posString, negString;
    // Use this for initialization
    void Start ()
    {
        game = GameManager.instance;
        player = PlayerManager.instance;
        ui = UI.instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /*for enemies
     * 1.Increase Enemy Health
     * 2.Increase Enemy Attack
     * 3.Increase Enemy Count
     * 4.Activate Skeleton projectile attack
     * */
    void IncreaseEnemyHealth() //increase enemy health value. Each time this is the drawn negative bonus, the amount of the health increase is increased by 1.
    {
        enemyHealthIncrease += 1;
        game.enemyHealthMod += enemyHealthIncrease;
        negString = "Enemies have more health";

    }
    void IncreaseEnemyAttack()//increase enemy attack value. //Each time this is the drawn negative bonus,the amount of the attack increase is increased by 1.
    {
        enemyAttackIncrease += 1;
        game.enemyAttackMod += enemyAttackIncrease;
        negString = "Enemies are stronger";
    }
    void IncreaseEnemyCount()//increase enemy count per level by 1. maybe add a max value?
    {
        game.enemiesPerLevel+= 1;
        negString = "Enemies are more numerous";
    }
    void ActivateBoneAttack() //activates the skeletons bone attack
    {
        boneAttack = true;
        negString = "Skeletons can now throw things at you";
    }
    /*for the player
     * 1.Increase Player Health
     * 2.Increase Player Attack
     * 
     **/

    void IncreasePlayerHealth()//increase player health value.Each time this is drawn, the amount of the health increase is increased by 1.
    {
        playerHealthIncrease += 1;
        player.maxHealth +=playerHealthIncrease;
        posString = "Health";
    }
    void IncreasePlayerAttack()
    {
        player.attack += 1;
        posString = "Attack";
    }
    void extraSpin()//NOT IMPLIMENTED //gain the option to roll for a third mod, can be good or bad, and it's effect wil be doubled. //can only land on this bonus once. 
    {
        //thirdRoll = true;
    }

    public void RollMods()
    {
        int pMod = Random.Range(1, posCount + 1);
        int nMod = Random.Range(1, negCount + 1);
        ActivateMods(pMod, nMod);
        
    }
    void ActivateMods(int pMod,int nMod)
    {
        //activate negative mod
        switch (nMod)
        {
            case 1:
                {
                    IncreaseEnemyHealth();
                    break;
                }
            case 2:
                {
                    IncreaseEnemyAttack();
                    break;
                }
            case 3:
                {
                    IncreaseEnemyCount();
                    break;
                }
            case 4:
                {
                    if(boneAttack==true)//this has already been activated. reroll
                    {
                        RollMods();
                        return;
                    }
                    ActivateBoneAttack();
                    break;
                }
        }

        switch (pMod)
        {
            case 1:
                {
                    IncreasePlayerHealth();
                    break;
                }
            case 2:
                {
                    IncreasePlayerAttack();
                    break;
                }
        }
        ui.ShowBonus(posString, negString);
    }
}
