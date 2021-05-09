using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : MonoBehaviour
{
    //SLIME
    //COMPONENTS
    public Rigidbody2D slimeRigidbody;
    public SpriteRenderer slimeRenderer;
    public Animator slimeAnim;
    public GameObject playerObject;

    //VARIABLES
    public float targetDistance;
    public int speed;
    public int health = 100;
    public int enemyDamage = 10;
    public bool isAlive;
    public float[] XDamage = new float[2];




    void Start()
    {
        slimeRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update

    private void Awake()
    {
        // GameObject player = GameObject.FindGameObjectWithTag("Player");
        slimeAnim = GetComponent<Animator>();
        slimeAnim.SetBool("IsAlive", true);
    }

    //This script simply makes the slime follow the player if the players in range.
    public void FollowPlayer()
    {
        targetDistance = 50;

        if (playerObject != null)
        {
            if (Vector3.Distance(transform.position, playerObject.transform.position) < targetDistance)
            {
                if (transform.position.x < playerObject.transform.position.x)
                {
                    slimeRenderer.flipX = false;
                    speed = 2;
                    slimeRigidbody.velocity = new Vector2(speed, slimeRigidbody.velocity.y);
                   
                }
                else if (transform.position.x > playerObject.transform.position.x)
                {
                    slimeRenderer.flipX = true;
                    speed = 2;
                    slimeRigidbody.velocity = new Vector2(-speed, slimeRigidbody.velocity.y);
                   
                }
            }
        }
    }

    //Slime deals damage to player on collision. It passes its cordinate for knock back
    void OnCollisionEnter2D(Collision2D coll)
    {
        {
            XDamage[0] = this.transform.position.x;
            XDamage[1] = enemyDamage;
            if (coll.gameObject.tag == "Player")
            {

                coll.gameObject.SendMessageUpwards("TakeDamage", XDamage);
            }
        }
    }

    //Allows the player to call, and pass current amount of damage to any enemy as all of them have TakeDamage class
    public void TakeDamage(int playerDamage)
    {
        float disableTime = 0.8f;
        DisableFollow(disableTime);

        if (transform.position.x > playerObject.transform.position.x)
        { 
            slimeRigidbody.velocity = new Vector2(6f, slimeRigidbody.velocity.y);
            slimeRigidbody.AddForce(new Vector2(0f, 12f), ForceMode2D.Impulse);
        }
        else
        {
            slimeRigidbody.velocity = new Vector2(-6f, slimeRigidbody.velocity.y);
            slimeRigidbody.AddForce(new Vector2(0f, 12f), ForceMode2D.Impulse);
        }
        
        health -= playerDamage;
        if(health <= 0)
        {
            OnDeath();
        }
    }
     public void OnDeath()
    {
       //Disables colliders and plays death animation
        slimeAnim.SetBool("IsAlive", false);
        this.GetComponent<BoxCollider2D>().enabled = false;
		this.GetComponent<CircleCollider2D>().enabled = false;

		Destroy(gameObject,1.2f);
        this.GetComponent<SlimeScript>().enabled = false;



    }

    //This is used to stop the slime moving toward the player when the slime is knocked back
    public void DisableFollow(float disableTime)
    {
        //This isnt my code.
        enabled = false;
        // If is called multiple times, reset timer.
        CancelInvoke("Enable");
        // Note: Even if we have disabled the script, Invoke will still run.
        Invoke("EnableFollow", disableTime);
    }
    
    public void EnableFollow()
    {
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
       FollowPlayer();
    }
}
