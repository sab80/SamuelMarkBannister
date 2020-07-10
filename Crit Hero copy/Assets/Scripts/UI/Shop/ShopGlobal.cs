using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopGlobal : MonoBehaviour
{
    public static ShopGlobal Instance;
    // Saved variables to be carried over scenes.
    public int nextHealthUpgrade;
    public int nextAttackUpgrade;
    public int nextCritUpgrade;

    public string GetToString()
    {
        return nextHealthUpgrade.ToString() + "|" + nextAttackUpgrade.ToString() + "|" + nextCritUpgrade.ToString();
    }

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