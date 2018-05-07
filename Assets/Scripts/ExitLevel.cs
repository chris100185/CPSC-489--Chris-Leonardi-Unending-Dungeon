using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLevel : MonoBehaviour {

    GameManager game;
    bool loadingLevel=false;//ensures that it only tries to load a level once
    public AudioClip door;
	// Use this for initialization
	void Start ()
    {
        game = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType() == typeof(BoxCollider2D))
        {
            if (collision.gameObject.tag == "Player")
            {
                if (!loadingLevel)
                {
                    loadingLevel = true;
                    game.NextLevel();
                }
            }
        }
    }
    public void OpenDoor()
    {
        GetComponent<AudioSource>().PlayOneShot(door);
    }
}
