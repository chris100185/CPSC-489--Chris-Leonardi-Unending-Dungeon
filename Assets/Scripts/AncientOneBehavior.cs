using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//script for controlling the basic AI of the Ancient One boss
/*
 * Basic Behavior:
 *  Ancient One remains stationary, 
 *  He continuous fires orbs in random directions from his back at a high rate of speed and spawn. These trigger collide and cause high damage when touched by player
 *  His second attack is a beam fired directly in front of him. this trigger collides and causes fast continuous damage while player is standing in it. 
 */
public class AncientOneBehavior : MonoBehaviour {
    private Animator anim; //The parent animator
    Boss boss;

    //boss speciffic behavior
    public int fireRate = 2; //how often the boss fires off his attack.
    float fireCountdown; //gets set to fireRate and then decremented, on 0 enter attack cycle;
    public float attackSpeed=0.5f; //The time interval between attacks in an attack cycle
    float fireSpeedCountdown; //gets set to attack speed and then decremented, on 0 fire an attack volley
    public bool attack1Active;
    public GameObject[] attacks;
    GameObject generator1, generator2, generator3, generator4; //the generators for attack 1

    // Use this for initialization
    void Start ()
    {
        anim = transform.GetComponent<Animator>();
        boss = GetComponent<Boss>();
        generator1 = GameObject.Find("Generator1");
        generator2 = GameObject.Find("Generator2");
        generator3 = GameObject.Find("Generator3");
        generator4 = GameObject.Find("Generator4");
        fireCountdown = fireRate;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //we don't want to do any of this if the boss is dead
        if (!boss.bossDead)
        {
            //if rate of fire timer met, attack again. don't do this if currently attacking
            if (!attack1Active)
            {
                fireCountdown -= Time.deltaTime;
                if (fireCountdown <= 0)
                {
                    anim.SetBool("Attack1", true);
                    attack1Active = true;
                    fireSpeedCountdown = attackSpeed;
                    fireCountdown = fireRate;
                }
            }
            if (attack1Active)
            {
                fireSpeedCountdown -= Time.deltaTime;
                if (fireSpeedCountdown <= 0)
                {
                    Attacking();
                    Attacking(); //generate two waves per attack cycle
                    fireSpeedCountdown = attackSpeed;
                }
            }
        }
    }

    //create the attack bolts
    void Attacking()
    {   
        GameObject bolt1 = Instantiate(attacks[0]);
        bolt1.transform.position = generator1.transform.position;
        GameObject bolt2 = Instantiate(attacks[0]);
        bolt2.transform.position = generator2.transform.position;
        GameObject bolt3 = Instantiate(attacks[0]);
        bolt3.transform.position = generator3.transform.position;
        GameObject bolt4 = Instantiate(attacks[0]);
        bolt4.transform.position = generator4.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType() == typeof(CapsuleCollider2D)&&collision.gameObject.tag=="Player")
        {
            boss.DamageBossFromPlayer();
        }
    }
}
