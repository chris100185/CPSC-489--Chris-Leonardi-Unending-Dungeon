using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public int enemyCount = 0;
    bool roomCleared = false;
    GameManager gameManager;
    PlayerManager player;
    List<GameObject> enemies = new List<GameObject>();
    public GameObject[] spawnZones;



    // Called on start of Level. Generate enemies and apply any level mods. 
    void Start()
    {
        gameManager = GameManager.instance;
        player = PlayerManager.instance;
        print("Starting Level");
        enemyCount += gameManager.enemiesPerLevel;
        print("There are " + enemyCount + " enemies in this level");
        SpawnEnemies(gameManager.enemiesPerLevel);
        player.Spawn();

    }

    // Update is called once per frame
    void Update()
    {

        if (enemyCount <= 0)//all enemies dead trigger end of level
        {
            GameObject door = GameObject.Find("Exit");
            if (door == null)
            {
                print("Error: door not found");
                return;
            }
            else
            {
                //print("found a door named "+door.name+"with tag:" + door.tag); //ui is conflicting with the exit door(exit button)
                if (!roomCleared)
                {
                    roomCleared = true;
                    Animator doorAnim = door.GetComponent<Animator>();
                    BoxCollider2D doorCollider = door.GetComponent<BoxCollider2D>();
                    doorAnim.SetBool("LevelClear", true);
                    doorCollider.isTrigger = true;
                    door.GetComponent<ExitLevel>().OpenDoor();
                    //you beat the room, have some health back
                    player.Healing(15);
                }

            }
        }

    }
    //custom methods
    void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            print("Spawning enemies");
            GameObject newEnemy;
            int newEnemyIndex = Random.Range(0, gameManager.enemies.Length);
            newEnemy = Instantiate(gameManager.enemies[newEnemyIndex]);
            enemies.Add(newEnemy);
        }
    }

    public void KillAllEnemies()//used to kill all enemies on the screen, should only be used when a boss is 
    {
        for(int i=enemies.Count-1;i>=0;i--)
        {
            //Destroy(enemies[i]);
            if (enemies[i] == null)//ensure that we have an enemey
                continue;
            enemies[i].GetComponent<Animator>().SetBool("isDead", true);
           // enemyCount--;
        }
    }
}
