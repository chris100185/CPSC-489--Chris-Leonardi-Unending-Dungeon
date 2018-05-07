using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    public bool pitEnabled = true;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.active)
        {
            if (collision.GetType() == typeof(BoxCollider2D))
            {
               // player.DamagePlayer(2, false, true);
            }
        }
    }
}
