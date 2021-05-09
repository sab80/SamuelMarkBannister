using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIScript : MonoBehaviour
{
	public GameObject shopUI;
    private int initialHealthUpgrade;
    private int initialAttackUpgrade;
    private int initialCritUpgrade;

    private int nextAttackUpgrade;
    private int nextCritUpgrade;

    private int price;
    private string priceToString;
    private int initialPrice;
    private int upgradeCounter;
    


    // Start is called before the first frame update
    void Start()
    {
		shopUI.SetActive(false);
        initialHealthUpgrade = 20;
        initialAttackUpgrade = 20;
        initialCritUpgrade = 100;
        initialPrice = 300;
    }

    public void SetActive()
	{
		shopUI.SetActive(true);
		Time.timeScale = 0;
	}
    //These methods are all attacked to buttons, and add the upgrade to the player and deduct the price from the gold
    public void BuyHealth()
    {
        if (GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().GetPlayerGold() > price)
        {
            GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().SendMessageUpwards("SpendGold", price);
            GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().SendMessageUpwards("UpgradeMaxHealth", initialHealthUpgrade);
            GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().IncrementUpgrade();
        }
    }
    public void BuyAttack()
    {
        if (GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().GetPlayerGold() > price)
        {
            GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().SendMessageUpwards("SpendGold", price);
            GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().SendMessageUpwards("UpgradeAttack", nextAttackUpgrade);
            GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().IncrementUpgrade();
        }
    }
    public void BuyCrit()
    {
        if (GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().GetPlayerGold() > price)
        {
            GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().SendMessageUpwards("SpendGold", price);
            GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().SendMessageUpwards("UpgradeCrit", nextCritUpgrade);
            GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().IncrementUpgrade();
        }
    }

    public void ExitShop()
	{
		shopUI.SetActive(false);
		Time.timeScale = 1;
	}

    public int GetHealthUp()
    {
        return initialHealthUpgrade;
    }

      public int GetAttackUp()
    {
        return nextAttackUpgrade;
    }

      public int GetCritUp()
    {
        return nextCritUpgrade;
    }

    public string GetPriceToString()
    {
        return priceToString;
    }

    // Update is called once per frame
    void Update()
    {
        //Changes prices on buttons
       upgradeCounter = GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().GetUpgradeCounter();
       price = initialPrice + (initialPrice * upgradeCounter)/2;
       priceToString = price.ToString();
       
       //updates the next upgrades 
       nextAttackUpgrade = (initialAttackUpgrade * upgradeCounter);
     
       nextCritUpgrade = (initialAttackUpgrade * upgradeCounter);
      

    }
}
