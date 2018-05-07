using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public Text text;
    PlayerManager player;

	// Use this for initialization
	void Start ()
    {
        text = GetComponent<Text>();
        player = PlayerManager.instance;
	}
	
	// Update is called once per frame
	void Update ()
    {
        text.text ="Health: "+player.health.ToString();
	}
}
