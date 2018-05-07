using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        print("Loading Main Menu");
        //check if we're returning to the menu from game and if so destroy all persistant game objects for reloading on next start. 
        if (GameManager.instance != null)
            Destroy(GameManager.instance.gameObject);
        if (PlayerManager.instance != null)
            Destroy(PlayerManager.instance.gameObject);
        if (UI.instance != null)
            Destroy(UI.instance.gameObject);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void closeGame()
    {
        Application.Quit();
    }
    public void Credits(bool show)
    {
        GameObject title = GameObject.Find("Title");
        GameObject exit = GameObject.Find("Exit");
        GameObject start = GameObject.Find("Start");
        GameObject credits = GameObject.Find("Credits");
        GameObject creditsText = GameObject.Find("CreditsText");
        if (show)//show the credits
        {
            title.transform.localScale = new Vector3(0, 1, 1);
            exit.transform.localScale = new Vector3(0, 1, 1);
            start.transform.localScale = new Vector3(0, 1, 1);
            credits.transform.localScale = new Vector3(0, 1, 1);
            creditsText.transform.localScale = new Vector3(1, 1, 1);
        }
        else //hide the credits
        {
            title.transform.localScale = new Vector3(1, 1, 1);
            exit.transform.localScale = new Vector3(1, 1, 1);
            start.transform.localScale = new Vector3(1, 1, 1);
            credits.transform.localScale = new Vector3(1, 1, 1);
            creditsText.transform.localScale = new Vector3(0, 1, 1);
        }
    }
}
