using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCritChange : MonoBehaviour
{
    private string toBeDisplayed;
    private int playerCrit;
    private int nextCritUp;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerCrit = GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().GetPlayerCrit();
        nextCritUp = GameObject.Find("Shop-UI").GetComponent<ShopUIScript>().GetCritUp();
        toBeDisplayed = (playerCrit + " -- " + (playerCrit + nextCritUp)).ToString();
        this.gameObject.GetComponent<UnityEngine.UI.Text>().text = (toBeDisplayed);

    }
}
