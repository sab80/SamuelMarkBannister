using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceOnButton : MonoBehaviour
{
    public string priceToString;
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    priceToString = GameObject.Find("Shop-UI").GetComponent<ShopUIScript>().GetPriceToString();
    //    this.gameObject.GetComponent<UnityEngine.UI.Text>().text = (priceToString);
    //}


GameObject ShopUI;// Our refference to text component


private ShopUIScript shopScript;

void Start()
{
    ShopUI = GameObject.FindGameObjectWithTag("Shop");
    shopScript = ShopUI.GetComponent<ShopUIScript>();
}

private void Update()
{
    priceToString = shopScript.GetPriceToString();
    this.gameObject.GetComponent<UnityEngine.UI.Text>().text = (priceToString);
}
}
