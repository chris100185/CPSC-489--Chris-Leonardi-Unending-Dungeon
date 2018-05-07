using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//The game manager is used to store general game data that needs to be preserved accross levels, and handle general game functions. 
public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    //public int enemiesPerRoom; //the current number of enemies that should generate per room
    public GameObject[] enemies; //array containing the currently available enemies. enemySpawn will pull from this list.

    //gameplay modifications
    public bool boneThrowActive = false;
    public int enemiesPerLevel;


    //private
    private int levelRange = 2; //the total number of playable levels;
    //public int totalEnemies = 2
    public int enemyHealthMod = 0; //the current number of enemy health modifications active
    public int enemyAttackMod = 0; //the current nuber iof enemy attack power mods active. 
    //private int money = 0; //money is earned upon defeating enemies, can be spent in store
    private int enemiesRemaining = 1; //the number of enemies remaining in the current room, to be set on room creation

    public int roomsCleared = 1;//counts the number of rooms cleared, starts at 1 so boss room spawn doesn't work.


    LevelMods mods;
    //managers
    PlayerManager player;
    UI gameUI;




    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        player = PlayerManager.instance;
        gameUI = UI.instance;
        mods = GetComponent<LevelMods>();
    }



    // Update is called once per frame
    void Update() {

    }

    //custom methods
    public void NextLevel()
    {
        Debug.Log("Loading next level");
        player.active = false;
        roomsCleared += 1;
        int next = Random.Range(1, levelRange+1);
        if (roomsCleared %3 != 0)
            SceneManager.LoadScene("Level" + next.ToString());
        else //load boss room here
            SceneManager.LoadScene("Boss");
        //player.Spawn();
    }
    public void Restart()
    {

    }
    public void GameOver()
    {
        gameUI.GameOver();
    }
    public void changeEnemiesRemaining(int num)//the amount of enemies to add or subtract
    {
        enemiesRemaining += num;
    }
    public void ApplyMods()
    {
        mods.RollMods();
    }
}
