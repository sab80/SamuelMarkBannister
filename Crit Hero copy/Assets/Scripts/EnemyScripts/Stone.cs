using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public float[] XDamage = new float[2];
    public float stoneDamage = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))

        {
            XDamage[0] = this.transform.position.x;
            XDamage[1] = stoneDamage;
            collision.gameObject.GetComponent<PlayerController>().SendMessageUpwards("TakeDamage", XDamage);
            
        }
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
