using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoin : MonoBehaviour
{
    public BoxCollider2D Col;
    public SpriteRenderer coinRenderer;
    public AudioSource audioFile;
    public int goldValue = 100;

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
        //if the players collides with a coin the value is added to the players gold and the object is destroyed
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessageUpwards("AddCoins", goldValue);
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
