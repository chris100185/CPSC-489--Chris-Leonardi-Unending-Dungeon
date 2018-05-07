using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {


    Vector3 direction;
    public float speed;
    public int damage;
    PlayerManager player;
    GameManager game;

    // Use this for initialization
    void Start()
    {
        //pick a direction
        direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0f).normalized;
        //find managers
        player = PlayerManager.instance;
        game = GameManager.instance;


        //apply mods
        damage += game.enemyAttackMod;
    }
    public void setDirection(Vector3 dir) //by default projectile will fly off in a random direction. call this function on creation to override
    {
        direction = dir.normalized;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    private void OnBecameInvisible() //destroy the projectile when it leaves the screen
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") //the projectile has collided with the player, damage the player and destroy the projectile.
        {
            //print("collided with player and projectile");
            player.DamagePlayer(damage, true, false);
            Destroy(gameObject);
        }
    }
  
}
