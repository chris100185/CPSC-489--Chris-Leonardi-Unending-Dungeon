using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {
    public int health, attack,defense;
    public float speed;
    public float wanderLength; //the amount of time the enemy should wander before changing directions.
    
    public enum direction {up,down,left,right}
    public direction dir=direction.down;
    bool wanderLock = false; //set to true if enemy is in the middle of wandering cycle
    bool damageLock = false;//set to true if enemy is damaged to prevent moonwalking
    bool deathLock = false;//set to true if enemy is dead to prevent moonwalking
    private float wanderTime;//the amount of time remaining for the current wander cycle. when 0 unlock and reset
    private Animator anim; //The parent animator.

    public bool isDead = false;//flag to prevent certain actions from occuring on death.
    bool killingEnemy = false;//flag to ensure that the enemy kill function is only called once per enemy;NOTE: test later if only one event needed per blend tree.

    //managers
    LevelManager level;
    PlayerManager player;
    GameManager game;


    // Use this for initialization
    void Start ()
    {
        level = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        player = PlayerManager.instance;
        anim = transform.GetComponent<Animator>();
        game = GameManager.instance;

        //applyMods
        health += game.enemyHealthMod;
        attack += game.enemyAttackMod;

        //spawn enemy
        spawnEnemy();
    }
	
	// Update is called once per frame
	void Update () {
        //basic enemy movement. This will be default movement unless overriden by a seperate script specific to
        //that enemy.
        if (wanderTime <= 0) wanderLock = false;
        if (wanderLock == false)//the enemy has walked in a direction for the length of wanderlock;
        {
            int move = Random.Range(0, 4);//to determine direction of wandering movement. 0=up,1=down,2=left,3=right
            dir = (direction)move;
            wanderTime = wanderLength;
            wanderLock = true;
        }
        //decrement wanderTime;
        wanderTime -= Time.deltaTime;
        int hor, ver;
        hor = ver = 0; 
        //set values for movement 
        if(dir==direction.down)
        {
            hor = 0;
            ver = -1;
        }
        else if(dir==direction.up)
        {
            hor = 0;
            ver = 1;
        }
     
        else if(dir==direction.left)
        {
            hor = -1;
            ver = 0;
        }
        else if(dir==direction.right)
        {
            hor = 1;
            ver = 0;
        }
        //set animator values
        anim.SetFloat("posX", hor);
        anim.SetFloat("posY", ver);
        Vector3 movement = new Vector3(hor, ver,0);
        transform.position+= movement * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("Damaging player");
            player.DamagePlayer(attack, true, false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType() == typeof(CapsuleCollider2D))
        {           
           // Debug.Log("collided with "+ collision.gameObject.tag);
            if(collision.gameObject.tag=="Player")// attacked by hero
            {
                //  EnemyBehavior enemy = collision.gameObject.GetComponent<EnemyBehavior>();
                PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();
                anim.SetBool("isHurt", true);
                health -= player.attack;
                if(health<=0)
                {
                    if (isDead == false)
                    {
                        isDead = true;
                        anim.SetBool("isDead", true);
                        //level.enemyCount -= 1;
                        //KillEnemy();
                    }
                }
            }
        }
       
    }

    //custom methods

    //position the enemy on creation randomly onto the spawn zones
    void spawnEnemy()
    {
        bool valid = false;
        Vector3 point=new Vector3(0,0,0);
        while(!valid)
        {
            //valid = true;
            point = new Vector3(Random.Range(-6.5f, 6.5f), Random.Range(-4f, 2.5f),0.0f);
            for(int i=0;i<level.spawnZones.Length;i++)
            {                
                if (level.spawnZones[i].GetComponent<BoxCollider2D>().OverlapPoint(point))
                {                                    
                    valid = true;                    
                    break;
                }                
            }
        }
        transform.position = point;
    }

    private void KillEnemy()
    {
        if (!killingEnemy)
        {
            killingEnemy = true;
            print("Destroying Enemy: " + gameObject.name);
            level.enemyCount -= 1;
            Destroy(gameObject);
        }
    }
}