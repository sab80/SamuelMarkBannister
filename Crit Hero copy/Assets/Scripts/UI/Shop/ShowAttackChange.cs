using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAttackChange : MonoBehaviour
{
    private string toBeDisplayed;
    private int playerAttack;
    private int nextAttackUp;
    private int nextAttackDisplay;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        playerAttack = GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().GetPlayerAttack();
        nextAttackUp = GameObject.Find("Shop-UI").GetComponent<ShopUIScript>().GetAttackUp();
        nextAttackDisplay = playerAttack + nextAttackUp;
        Debug.Log("Current" + playerAttack);
        Debug.Log("Next up" + nextAttackUp);
        Debug.Log("Display" + nextAttackDisplay);
        toBeDisplayed = (playerAttack + " -- " + (nextAttackDisplay)).ToString();
        this.gameObject.GetComponent<UnityEngine.UI.Text>().text = (toBeDisplayed);

    }
}
