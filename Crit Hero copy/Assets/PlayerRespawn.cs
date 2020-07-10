using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{

    public GameObject playerGO;
    public PlayerController playerCont;
    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.Find("PlayerGameObject");
        playerCont = playerGO.GetComponent<PlayerController>();
    }

    public void RespawnPlayer()
    {
        // AsyncOperation asyncLoadLevel;
        StartCoroutine(DeathAnimDelay());
       


        //SceneManager.LoadSceneAsync("Main_1", LoadSceneMode.Single);
        // SceneManager.LoadScene("Main_1");

        //GameObject.Find("PlayerGameObject").GetComponent<Transform>().position = spawnPos;
        
    }

    IEnumerator DeathAnimDelay()
    {

        yield return new WaitForSeconds(2);
        Debug.Log("LoadingScene");
        SceneManager.LoadSceneAsync("Main_1", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
