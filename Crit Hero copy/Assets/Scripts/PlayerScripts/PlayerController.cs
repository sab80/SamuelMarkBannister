    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Player Stats saved in GlobalController
    public int playerHealth;
    public int playerMaxHealth;
    public int gold;
    public int attackDamage;
    public int critDamage;
    public float critChance;
    public int upgradeCounter;
    public Vector3 currentSpawnPoint;
    public Vector3 currentPlayerPosition;
    public bool ShopTreeActive;
    public bool GreenTreeActive;
    public bool BlueTreeActive;
    public bool BossTreeActive;
    public bool hasDoubleJump;
    public bool hasMap;

    //Components
    public GameObject GM;
    public Animation anim;
    public Animator animator;
    public GameObject EnemyObject;
    public Rigidbody2D PlayerRB;
    public Continue continueScript;
    //Passed from main menu
    public bool isLoadNeeded;

    //Getters
    public int GetPlayerHealth()
    {
        return playerHealth;
    }
    public int GetPlayerMaxHealth()
    {
        return playerMaxHealth;
    }
    public int GetPlayerGold()
    {
        return gold; 
    }
    public int GetPlayerAttack()
    {
        return attackDamage;
    }
    public int GetPlayerCrit()
    {
        return critDamage;
    }
    public float GetPlayerCritChance()
    {
        return critChance;
    }
    public int GetUpgradeCounter()
    {
        return upgradeCounter;
    }
    public void IncrementUpgrade()
    {
        upgradeCounter++;
    }
    // Start is called before the first frame update
    void Awake()
    {
        //initialises variables if its the first time running and GlobalController hasn't been used yet
        playerHealth = 100;
        playerMaxHealth = 100;
        gold = 0;
        attackDamage = 10;
        critDamage = 30;
        critChance = 0.2f;
        upgradeCounter = 1;
        //Check if ContinueGlobalSave has been passed through from the main menu.
        //This stops errors if there is not one
        if (GameObject.Find("ContinueGlobalSave"))
        {
            continueScript = GameObject.Find("ContinueGlobalSave").GetComponent<Continue>();
        }
        //Finds components needed
        GM = GameObject.Find("-GM");
        animator = GetComponent<Animator>();
        PlayerRB = this.gameObject.GetComponent<Rigidbody2D>();

        //For when the player dies.
        animator.SetBool("IsAlive", true);
        LoadFromGlobal();
        this.transform.position = currentSpawnPoint;
        ActivateTrees();
       
    }
    void Start()
    {
        //Checks if main menu has been used
        if (GameObject.Find("ContinueGlobalSave"))
        {
            if (!Continue.Instance.isnewGameNeeded && !Continue.Instance.isLoadNeeded)
            {
                //This code is ran when the player dies in game
                LoadFromGlobal();
                this.transform.position = currentSpawnPoint;
                ActivateTrees();
            }
            else if (Continue.Instance.isLoadNeeded)
            {
                //If the game is being loaded from file
                GameObject.Find("-GM").GetComponent<SaveLoad>().LoadFromFile();
                LoadFromGlobal();
                Continue.Instance.isLoadNeeded = false;
                //Spawns player at the position it was in when saved, instead of a travel tree
                this.transform.position = currentPlayerPosition;
                ActivateTrees();
            }
            else if (Continue.Instance.isnewGameNeeded)
            {
                //Allows no load, game is started from new
                Continue.Instance.isnewGameNeeded = false;
            }

        }
    }
    //This updates the playerControllers variables from the GlobalController
    public void LoadFromGlobal()
    {
        gold = GlobalControl.Instance.gold;
        playerHealth = GlobalControl.Instance.health;
        playerMaxHealth = GlobalControl.Instance.maxHealth;
        attackDamage = GlobalControl.Instance.attackDamage;
        hasDoubleJump = GlobalControl.Instance.hasDoubleJump;
        hasMap = GlobalControl.Instance.hasMap;
        critDamage = GlobalControl.Instance.critDamage;
        critChance = GlobalControl.Instance.critChance;
        currentSpawnPoint = GlobalControl.Instance.currentSpawnPoint;
        currentPlayerPosition = GlobalControl.Instance.currentPlayerPosition;
        ShopTreeActive = GlobalControl.Instance.ShopTreeActive;
        GreenTreeActive = GlobalControl.Instance.GreenTreeActive;
        BlueTreeActive = GlobalControl.Instance.BlueTreeActive;
        BossTreeActive = GlobalControl.Instance.BossTreeActive;
        upgradeCounter = GlobalControl.Instance.upgradeCounter;
}
    

    public void UpdateSpawn(GameObject SpawnGO)
    {
        currentSpawnPoint = SpawnGO.transform.position;
        Debug.Log(currentSpawnPoint);
    }

    public void AddCoins(int coinValue)
    {
        gold += coinValue;
    }

    public void SpendGold(int price)
    {
        gold -= price;
    }

    public void TakeDamage(float[] XDamage)
    {
        //used to disable the movement of the player
        float disableTime = 0.8f;
        this.gameObject.SendMessageUpwards("Disable", disableTime);
        //float is converted to int
        int enemyDamage = Convert.ToInt32(XDamage[1]);
        if (transform.position.x > XDamage[0])
        {
            //Adds force right and up
            PlayerRB.velocity = new Vector2(12f, PlayerRB.velocity.y);
            PlayerRB.AddForce(new Vector2(0f, 12f), ForceMode2D.Impulse);
        }
        else
        {
            //Adds force left and up
            PlayerRB.velocity = new Vector2(-12f, PlayerRB.velocity.y);
            PlayerRB.AddForce(new Vector2(0f, 12f), ForceMode2D.Impulse);
        }
  
        
     //Takes damage dealt to player from the players health, then checks if player has died
        playerHealth -= enemyDamage;
        if (playerHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        //Stops the player moving
        gold = 0;
        float disableTime = 3.0f;
        this.gameObject.GetComponent<PlayerMovement>().SendMessageUpwards("Disable", disableTime);
        PlayerRB.velocity = new Vector3(0,0,0);
        PlayerRB.bodyType = RigidbodyType2D.Static;
        currentPlayerPosition = this.transform.position;
        //Plays player death animation
        animator.SetBool("IsAlive", false);
        Respawn();

    }

    public void Respawn()
    {
        //Saves player stats to the global controller which wont be destroyed on scene change,before the scene is reset.
        SavePlayerStats();
        //Calls co-routine to handle delay for death animation
        StartCoroutine(DeathAnimDelay()); 
    }

    public void FullHeal()
    {
        playerHealth = playerMaxHealth;
    }

    public void UpgradeMaxHealth(int upgrade)
    {
        playerMaxHealth += upgrade;
    }
    public void UpgradeAttack(int upgrade)
    {
        attackDamage += upgrade;
    }
    public void UpgradeCrit(int upgrade)
    {
        critDamage += upgrade;
    }

    //Saves PlayerControllers variables to GlobalController
    public void SavePlayerStats()
    {
        currentPlayerPosition = this.gameObject.transform.position;
        GlobalControl.Instance.gold = gold;
        GlobalControl.Instance.health = playerHealth;
        GlobalControl.Instance.maxHealth = playerMaxHealth;
        GlobalControl.Instance.attackDamage = attackDamage;
        GlobalControl.Instance.hasDoubleJump = hasDoubleJump;

        GlobalControl.Instance.hasMap = hasMap;
        GlobalControl.Instance.critDamage = critDamage;
        GlobalControl.Instance.critChance = critChance;
        GlobalControl.Instance.currentSpawnPoint = currentSpawnPoint;
        GlobalControl.Instance.currentPlayerPosition = currentPlayerPosition;
        GlobalControl.Instance.ShopTreeActive = ShopTreeActive;
        GlobalControl.Instance.GreenTreeActive = GreenTreeActive;
        GlobalControl.Instance.BlueTreeActive = BlueTreeActive;
        GlobalControl.Instance.BossTreeActive = BossTreeActive;

    }
    //Re-activates trees that were active before scene change or load
    public void ActivateTrees()
    {
        if (ShopTreeActive)
        {
            GameObject.Find("ShopTravelTree").GetComponent<TreeScript>().Activate();
        }
        if (GreenTreeActive)
        {
            GameObject.Find("GreenTravelTree").GetComponent<TreeScript>().Activate();
        }
        if (BlueTreeActive)
        {
            GameObject.Find("BlueTravelTree").GetComponent<TreeScript>().Activate();
        }
        if (BossTreeActive)
        {
            GameObject.Find("BossTravelTree").GetComponent<TreeScript>().Activate();
        }
    }

    //Updates whether a tree is active
    public void AddActiveTree(GameObject treeGO)
    {
        switch (treeGO.name)
        {

            case "ShopTravelTree":
                ShopTreeActive = true;
                break;
            case "GreenTravelTree":
                GreenTreeActive = true;
                break;
            case "BlueTravelTree":
                BlueTreeActive = true;
                break;
            case "BossTravelTree":
                BossTreeActive = true;
                break;
            default:
                Debug.Log("Tree Not Found");
                break;
        }
    }


    //CO-Routines

    //waits for animation to player before changing scene
    public
    IEnumerator DeathAnimDelay()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("LoadingScene");
        SceneManager.LoadSceneAsync("Main_1", LoadSceneMode.Single);
    }

  





    // Update is called once per frame
    void Update()
    {
        currentPlayerPosition = this.gameObject.transform.position;
   
    }
}
