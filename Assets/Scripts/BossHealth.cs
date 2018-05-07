using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour {
    GameObject bossObj=null; //on boss spawn, the boss will link itself to this gameobject
    Boss boss;
    AncientOneBehavior ancient;
    public Slider health;
    

	// Use this for initialization
	void Start ()
    {
        //boss = GameObject.Find("The Ancient");
        //ancient = bossObj.GetComponent<AncientOneBehavior>();
        //health = GetComponent<Scrollbar>();		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (bossObj!=null)
            health.value = (float)((float)boss.health/ (float)boss.maxHealth);
	}

    public void LinkBoss(GameObject obj)
    {
        print("LinkBoss has received an object named: " + obj.name);
        bossObj = obj;
        boss = obj.GetComponent<Boss>();
    }
}
