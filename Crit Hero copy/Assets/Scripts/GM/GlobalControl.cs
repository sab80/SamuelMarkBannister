using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    //This is not my code, reference in report
    public static GlobalControl Instance;
    // Saved variables to be carried over scenes.

    public int gold;
    public int health;
    public int maxHealth;
    public bool hasDoubleJump;
    public bool hasMap;
    public int attackDamage;
    public int critDamage;
    public float critChance;
    public Vector3 currentSpawnPoint;
    public Vector3 currentPlayerPosition;
    public bool ShopTreeActive;
    public bool GreenTreeActive;
    public bool BlueTreeActive;
    public bool BossTreeActive;
	public int upgradeCounter;

    //This is called when saving. The string is set up ready for the variables to be saved appropriately
	public string GetToString()
    {
        return gold.ToString() + "|" + health.ToString() + "|" + maxHealth.ToString() + "|" + hasDoubleJump.ToString() + "|" + hasMap.ToString() + "|" + attackDamage.ToString() + "|" + critDamage.ToString() + "|" + critChance.ToString() + "|" + currentSpawnPoint.ToString() + "|" + currentPlayerPosition.ToString() + "|" + ShopTreeActive.ToString() + "|" + GreenTreeActive.ToString() + "|" + BlueTreeActive.ToString() + "|" + BossTreeActive.ToString() + "|" + upgradeCounter.ToString() + "^";
    }

    //Destroys any other instance of the class, so there is only ever one
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
