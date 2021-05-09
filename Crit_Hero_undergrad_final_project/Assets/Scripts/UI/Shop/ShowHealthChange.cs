using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHealthChange : MonoBehaviour
{
    private string toBeDisplayed;
    private int playerMH;
    private int nextHealthUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerMH = GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().GetPlayerMaxHealth();
        nextHealthUp = GameObject.Find("Shop-UI").GetComponent<ShopUIScript>().GetHealthUp();
        toBeDisplayed = (playerMH + " -- " + (playerMH + nextHealthUp)).ToString();
        this.gameObject.GetComponent<UnityEngine.UI.Text>().text = (toBeDisplayed);
       
    }
}