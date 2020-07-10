using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public GameObject GMInst;
    public SaveLoad saveload;
    public GameObject PauseUi;
    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        PauseUi.SetActive(false);
    }

    private void Awake()
    {
        GMInst = GameObject.Find("-GM");
        saveload = GMInst.GetComponent<SaveLoad>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            Debug.Log("PAUSED");
            isPaused = !isPaused;    
        }

        if (isPaused)
        {
            PauseUi.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!isPaused)
        {
            PauseUi.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Resume()
    {
        isPaused = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main_1");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void Quit()
    {
        Debug.Log("QuitPressed");
        GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().SavePlayerStats();
        saveload.SaveToFile();
        Application.Quit();
    }
}
