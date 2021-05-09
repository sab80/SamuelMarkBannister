using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonScript : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D demonRB;
    public bool playerOnRight;
    public GameObject playerGO;
    public GameObject hitBox;

    public int moveSpeed;
    public int targetDistance = 5;
    public bool canAttack;
    public int health;
    
   

    //These flip the demon and the hitbox to face left or right
    public void MovingLeft()
    {
        gameObject.transform.localScale = new Vector3(-1, 1, 1);
    }

    public void MovingRight()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public bool GetPlayerDirection()
    {
        if (transform.position.x < playerGO.transform.position.x)
        {
            playerOnRight = true;
        }
        else
        {
            playerOnRight = false;
        }
        return playerOnRight;
    }
    void Start()
    {
        //Deactivates hitboxes at the start of the game
        GameObject.Find("DemonHitBox").SetActive(false);
        canAttack = true;
        anim.SetBool("IsAlive", true);
    }


    public void TakeDamage(int playerDamage)
    {
            StartCoroutine(Hurt());
            health -= playerDamage;
            if (health <= 0)
            {
                StopAllCoroutines();
                StartCoroutine(Death());
            }
        }

    //Moves the demon in the correct direction toward the player
    public void Movement()
    {
        
        if (playerOnRight)
        {

            anim.SetBool("Moving", true);
            MovingRight();
            demonRB.velocity = new Vector2(moveSpeed, demonRB.velocity.y);

        }
        else
        {
            MovingLeft();
            
            anim.SetBool("Moving", true);
            demonRB.velocity = new Vector2(-moveSpeed, demonRB.velocity.y);
            
        }
    }

    
    //Checks if the demon is allowed to attack
    public void CheckAttack()
    {
        if (Vector3.Distance(transform.position, playerGO.transform.position) < targetDistance)
        {
            if (canAttack)
            {
                StartCoroutine(Attack());
            }
        }
        //If the demon cant attack but the player is in distance to move toward, then the demon moves toward the player
        if ((Vector3.Distance(transform.position, playerGO.transform.position) < (targetDistance * 4)) && (Vector3.Distance(transform.position, playerGO.transform.position) > targetDistance))
        {
            Movement();
        }
        else
        {
            anim.SetBool("Moving", false);
            demonRB.velocity = new Vector2(0, demonRB.velocity.y);
        }
      

        
       
      
    }

    //Co-routines
    //Plays attacking animation and enables the hitboxes for a given time
    public
    IEnumerator Attack()
    {
        
        demonRB.velocity = new Vector2(0, demonRB.velocity.y);
        anim.SetBool("Attacking", true);
        hitBox.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        anim.SetBool("Attacking", false);
        hitBox.SetActive(false);
        StartCoroutine(AttackCooldown());

    }
    //Waits two seconds before the demon is allowed to attack again
    public
    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(2);
        canAttack = true;
    }

    //Plays the hurt animation when attacked
    public
    IEnumerator Hurt()
    {
        anim.SetBool("isHurt", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isHurt", false);
    }

    //Plays the death animation and disables colliders of the enemies
    public
    IEnumerator Death()
    {
        anim.SetBool("IsAlive", false);
        demonRB.velocity = new Vector2(0, 0);
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerDirection();
        CheckAttack();
    }
}
