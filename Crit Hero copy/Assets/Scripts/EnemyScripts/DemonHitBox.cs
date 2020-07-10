using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonHitBox : MonoBehaviour
{
    public float[] XDamage = new float[2];
    public float damage = 25;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            XDamage[0] = this.gameObject.transform.position.x;
            XDamage[1] = damage;
            collision.GetComponent<PlayerController>().SendMessageUpwards("TakeDamage", XDamage);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
