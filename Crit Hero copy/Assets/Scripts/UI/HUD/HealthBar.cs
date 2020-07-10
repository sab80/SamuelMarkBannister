using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject PlayerObject;
    public int playerHealth;
    public int playerMaxHealth;
    public PlayerController playerScript;
    public Animator healthAnim;

    
    // Start is called before the first frame update
    private void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        //Initiates the variables with the correct components
        playerScript = PlayerObject.GetComponent<PlayerController>();
        playerHealth = playerScript.GetPlayerHealth();
        playerMaxHealth = playerScript.GetPlayerMaxHealth();
        
    }

    public void updateHealth()
    {
        playerHealth = playerScript.GetPlayerHealth();
        playerMaxHealth = playerScript.GetPlayerMaxHealth();
        //Passes the players health as a percentage to the animator
        healthAnim.SetFloat("PlayerHealthPer",(float)playerHealth/playerMaxHealth * 100);
        
    }

    public void PositionOfBar()
    {
        if (PlayerObject != null)
        {
            this.transform.position = new Vector3(PlayerObject.transform.position.x - 7, PlayerObject.transform.position.y + 5.5f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Checks for the player position and the players health to be updated
        updateHealth();
        PositionOfBar();
    }
}
