using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuStartButton : MonoBehaviour
{
    public GameObject GMInst;
    public SaveLoad saveload;
    //text file path, used to reset the file to blank when the newgame is called
    public string path = "Assets/Resources/GameSave.txt";
    private void Awake()
    {
        GMInst = GameObject.Find("-GM");
        saveload = GMInst.GetComponent<SaveLoad>();
    }
    public void NewGame()
    {
        //passes to the main game scene
        Continue.Instance.isLoadNeeded = false;
        Continue.Instance.isnewGameNeeded = true;
        //Makes the file blank
        File.WriteAllText(path, String.Empty);  
        SceneManager.LoadScene("Main_1");
    }

    public void ContinueToGame()
    {
        // passes to the main game scene
        Continue.Instance.isLoadNeeded = true;
        Continue.Instance.isnewGameNeeded = false;
        SceneManager.LoadScene("Main_1");

    }

    public void QuitGame()
    {
        //Saves the players stats
        GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().SavePlayerStats();
        saveload.SaveToFile();
        Application.Quit();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ContinueToGame();
    }
}
