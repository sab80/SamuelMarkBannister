using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public ShopUIScript shopUI;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {

        shopUI= GameObject.Find("Shop-UI").GetComponent<ShopUIScript>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitBox"))
        {
            Debug.Log("trigs");
            shopUI.SendMessageUpwards("SetActive");
        }
    }
// Update is called once per frame
void Update()
    {
        
    }
}
