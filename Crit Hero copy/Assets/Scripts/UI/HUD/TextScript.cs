using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScript : MonoBehaviour
{
    GameObject PlayerObject;

    private int coinValue;
    private PlayerController playerScript;

    void Start()
    { 
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        playerScript = PlayerObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
        //This updates the text with the current amount of gold that needs to be displayed
        coinValue = playerScript.gold;
        this.gameObject.GetComponent<UnityEngine.UI.Text>().text = (coinValue.ToString());
    }
}
