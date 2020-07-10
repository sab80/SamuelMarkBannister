using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TravelTreeUi : MonoBehaviour
{
    public GameObject travelTreeUi;
    //GameObjects
    public GameObject PlayerOB;
    public GameObject ShopTreeOB;   
    public GameObject GreenTreeOB;
    public GameObject BlueTreeOB;
    public GameObject BossTreeOB;
    //The GameObjects that hold the button component
    public GameObject ShopPaths;
    public GameObject GreenPaths;
    public GameObject BluePaths;
    public GameObject BossPaths;
    //Each trees TreeScript
    public TreeScript ShopScript;
    public TreeScript GreenScript;
    public TreeScript BlueScript;
    public TreeScript BossScript;

    //Buttons for each tree
    private Button shopButton;
    private Button blueButton;
    private Button greenButton;
    private Button bossButton;

    //Deactivates the UI when the game first starts
    private bool travelMapActive = false;
    
    void Start()
    {
  
    }

    void Awake()
    {
        //Initalises variables
        if (GameObject.Find("Player") != null) { PlayerOB = GameObject.Find("PlayerGameObject"); }

        ShopScript = ShopTreeOB.GetComponent<TreeScript>();
        GreenScript = GreenTreeOB.GetComponent<TreeScript>();
        BlueScript = BlueTreeOB.GetComponent<TreeScript>();
        BossScript = BossTreeOB.GetComponent<TreeScript>();

        ShopPaths = GameObject.Find("ShopPaths");
        BluePaths = GameObject.Find("BluePaths");
        GreenPaths = GameObject.Find("GreenPaths");
      
        shopButton = GameObject.Find("ShopPaths").GetComponent<Button>();
        greenButton = GameObject.Find("GreenPaths").GetComponent<Button>();
        blueButton = GameObject.Find("BluePaths").GetComponent<Button>();
      
    }

   
    //Is called when the tree object is hit by the player, to open the UI
    public void ActivateTravelMap()
    {
        travelMapActive = true;
    }

    //These are functions called when the connected button is clicked
    public void ShopTree()
    {
        //Checks if the tree is active, if it is the player is teleported to it
        if (ShopScript.treeActive)
         { 
        PlayerOB.transform.position = ShopTreeOB.transform.position;
        travelMapActive = false;
         }
    }

    public void GreenTree()
    {
        //Checks if the tree is active, if it is the player is teleported to it
        if (GreenScript.treeActive)
        {
            PlayerOB.transform.position = GreenTreeOB.transform.position;
            travelMapActive = false;
        }

    }

    public void BlueTree()
    {
        //Checks if the tree is active, if it is the player is teleported to it
        if (BlueScript.treeActive)
        {
            PlayerOB.transform.position = BlueTreeOB.transform.position;
            travelMapActive = false;
        }
    }

    public void BossTree()
    {
        //Checks if the tree is active, if it is the player is teleported to it
        if (BossScript.treeActive)
        {
            PlayerOB.transform.position = BossTreeOB.transform.position;
            travelMapActive = false;
        }
    }


    void Update()
    {
        //These makes the inactive tree's corresponding buttons disabled
        if (!ShopScript.treeActive)
        {
            shopButton.interactable = false;

        }else
        {
            shopButton.interactable = true;
        }

        if (!BlueScript.treeActive)
        {
            blueButton.interactable = false;
        }
        else
        {
            blueButton.interactable = true;
        }

        if (!GreenScript.treeActive)
        {
             greenButton.interactable = false;
        }
        else
        {
            greenButton.interactable = true;
        }

        //Checks if the UI is active
        if (travelMapActive)
        {
            //When active, the game is paused
            travelTreeUi.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!travelMapActive)
        {
            //When deactived the game un-pauses
            travelTreeUi.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
