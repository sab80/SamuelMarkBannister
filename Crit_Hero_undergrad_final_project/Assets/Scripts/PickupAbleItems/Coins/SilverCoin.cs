using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverCoin : MonoBehaviour
{
    public int silverValue = 50;
    public BoxCollider2D Col;
    public SpriteRenderer coinRenderer;
    public AudioSource audioFile;
    
    private void Awake()
    {
        Col = GetComponent<BoxCollider2D>();
        audioFile = GetComponent<AudioSource>();
        coinRenderer = GetComponent<SpriteRenderer>();
        coinRenderer.enabled = true;
        Col.enabled = true;
    }
   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessageUpwards("AddCoins", silverValue);
            audioFile.Play();
            coinRenderer.enabled = false;
            Col.enabled = false;
            Destroy(1f);
        }

    }

    public void Destroy(float delay)
    {
        Destroy(gameObject, delay);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
