﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().hasDoubleJump = true;
            Destroy(this.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
