using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public static UI instance=null;

    GameObject gameOver;
    GameObject modScreen;
    public GameObject posMod;
    public GameObject negMod;
    float modTimer = 3;//flash the earned bonuses on the screen briefly after a boss has been killed.
    float modCountdown =0;
    bool modsOnScreen = false;
 
   public GameObject bossHealth;


    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        // Spawn();
    }

    void Start()
    {
        gameOver = GameObject.Find("Game Over");
        gameOver.transform.localScale=new Vector3(0,1,1);
        bossHealth = GameObject.Find("Boss Health");
        modScreen = GameObject.Find("Mod Screen");
        posMod = GameObject.Find("PosText");
        negMod = GameObject.Find("NegText");
        if(posMod==null)
        {
            Debug.LogError("Error finding posMod");
        }
        if(negMod==null)
        {
            Debug.LogError("Error finidng negMod");
        }

        BossHealthVisibility(false);
    }

    // Update is called once per frame
    void Update()
    {
        //calculate the remaining time on displaying mod timer
        if(modsOnScreen)
        {
            modCountdown -= Time.deltaTime;
            if(modCountdown<=0)
            {
                modsOnScreen = false;
                modScreen.transform.localScale = new Vector3(0, 1, 1);
            }
        }

    }
    public void GameOver()
    {
        gameOver.transform.localScale = new Vector3(1, 1, 1);
    }
    public void BossHealthVisibility(bool visible)
    {
        print("Setting Boss health Visibility to " + visible);
        if(visible)
            bossHealth.transform.localScale = new Vector3(1, 1, 1);
        else
            bossHealth.transform.localScale = new Vector3(0, 1, 1);
    }
    public void ShowBonus(string posBonus,string negBonus)
    {
        posMod.GetComponent<Text>().text = "You got a bonus to: " + posBonus;
        negMod.GetComponent<Text>().text = negBonus;
        modScreen.transform.localScale = new Vector3(1, 1, 1);
        modsOnScreen = true;
        modCountdown = modTimer;
    }
}
