using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class playerPit : MonoBehaviour {
    PlayerManager player;

    // Use this for initialization
    void Start ()
    {
        player = PlayerManager.instance;

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "pit")//we've touched the collison box set up so enemies can't walk through pits
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "pit") //player has entered a pit. This should hurt
        {
            player.DamagePlayer(2, false, true);
        }
    }
}
