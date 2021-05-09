using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    //Variables
    public int health = 1000;
    public Animator anim;
    public Rigidbody2D bossRB;

    public GameObject throwPoint;
    public GameObject stone;
    public int normalSpeed;
    public int chargeSpeed;
    public int currentSpeed;
    public int vunerableMoveSpeed;
    public bool isVunerable;
    public GameObject playerGO;
    public int randomAttack;
    public bool playerOnRight;
    public bool collidersDoDamage;
    public bool fightStarted;

    public float damage;
    public float chargeDamage;
    public float stoneDamage;
    public float spearDamage;
    public float[] XDamage = new float[2];
    // Start is called before the first frame update
    void Start()
    {
   
    }

    //Starts the attack sequence
    public void StartFight()
    {
        AttackHandler();
        fightStarted = true;
    }

    //Does damage to the player on collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collidersDoDamage && collision.gameObject.tag == "Player")
        {
            XDamage[0] = this.transform.position.x;
            XDamage[1] = damage;
            collision.gameObject.SendMessageUpwards("TakeDamage", XDamage);
        }
    }

    //Takes damage from the player
    public void TakeDamage(int playerDamage)
    {
        if (isVunerable)
        {
            StartCoroutine(Hurt());
            health -= playerDamage;
            if (health <= 0)
            {
                StopAllCoroutines();
                StartCoroutine(Death());
            }
        }
      
    }


    public void MovingLeft()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void MovingRight()
    {
        gameObject.transform.localScale = new Vector3(-1, 1, 1);
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

    //Randomly starts and attack co-routine
    public void AttackHandler()
    {
        StopAllCoroutines();
        Debug.Log("Handler called");
        randomAttack = Random.Range(0, 5);
        switch (randomAttack)
        {
            case 1:
                StartCoroutine(WalkingVunerable());
                Debug.Log("walking vunerable called");
                break;
            case 2:
                StartCoroutine(SpearThrust());
                Debug.Log("Spear called");
                break;
            case 3:
                StartCoroutine(Charge());
                Debug.Log("charge called");
                break;
            case 4:
                StartCoroutine(StoneThrow());
                Debug.Log("Stone called");
                break;
            default:
                AttackHandler();
                break;

        }
        

    }

    //Co-Routines
    //Allows the boss to be vulnerable to attacks, walks slowly away from the player
    public
    IEnumerator WalkingVunerable()
    {
        collidersDoDamage = false;
        Debug.Log("Walking Vunerale");
        isVunerable = true;
        anim.SetBool("isMoving", true);
        if (playerOnRight)
        {
            MovingLeft();
            bossRB.velocity = new Vector2(-vunerableMoveSpeed, bossRB.velocity.y);

        }
        else
        {
            bossRB.velocity = new Vector2(vunerableMoveSpeed, bossRB.velocity.y);
            MovingRight();
        }
        yield return new WaitForSeconds(5);
        bossRB.velocity = new Vector2(0, bossRB.velocity.y);
        anim.SetBool("isMoving", false);
        AttackHandler();
        
        

    }
    //plays boss hurt aninmation
    public
IEnumerator Hurt()
    {
        anim.SetBool("isHurt", true);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isHurt", false);



    }
    //Plays thrust animation
    public
    IEnumerator SpearThrust()
    {
        bossRB.velocity = new Vector2(0, bossRB.velocity.y);
        damage = spearDamage;
        isVunerable = false;
        collidersDoDamage = true;
        if (!playerOnRight)
        {
            MovingLeft();
            bossRB.velocity = new Vector2(-normalSpeed, bossRB.velocity.y);
        }
        else
        {
            MovingRight();
            bossRB.velocity = new Vector2(normalSpeed, bossRB.velocity.y);
        }
        anim.SetBool("isThrusting", true);
        yield return new WaitForSeconds(1);
        anim.SetBool("isThrusting", false);
        AttackHandler();
        
        
    }

    //Charges at the player while invulnerable, does this for 3 seconds
    public
    IEnumerator Charge()
    {
        damage = chargeDamage;
        isVunerable = false;
        collidersDoDamage = true;
        currentSpeed = chargeSpeed;
        if (!playerOnRight)
        {
            MovingLeft();
            bossRB.velocity = new Vector2(-chargeSpeed, bossRB.velocity.y);

        }
        else
        {
            bossRB.velocity = new Vector2(chargeSpeed, bossRB.velocity.y);
            MovingRight();
        }
        anim.SetBool("isCharging", true);
        yield return new WaitForSeconds(3);
        bossRB.velocity = new Vector2(0, bossRB.velocity.y);
        anim.SetBool("isCharging", false);
        AttackHandler();

    }

    // Throws a stone projectile at the player
    public
    IEnumerator StoneThrow()
    {
        bossRB.velocity = new Vector2(0, bossRB.velocity.y);
        isVunerable = true;
        collidersDoDamage = false;
        anim.SetBool("isThrowing", true);

        //Instantiate an new stone game object at the bosses hand
        GameObject newStone = Instantiate(stone, throwPoint.transform.position, new Quaternion(0f,0f,0f,0f));
        Rigidbody2D newStoneRB = newStone.GetComponent<Rigidbody2D>();
        
        if (playerOnRight)
        {
            MovingRight();
            //Moves the stone towards the player
            newStoneRB.AddForce(new Vector2(60f, 0f));
            
        }
        else
        {
            MovingLeft();
            newStoneRB.AddForce(new Vector2(-60f, 0f));
            
        }
        yield return new WaitForSeconds(1);
        anim.SetBool("isThrowing", false);
        AttackHandler();
   


    }

    //Plays death animation and destroys the gameobject
    public
IEnumerator Death()
    {
        collidersDoDamage = false;
        anim.SetBool("isDead", true);
        bossRB.velocity = new Vector2(0, 0);
        gameObject.GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if the boss should start its attack sequences
        if (Vector3.Distance(transform.position, playerGO.transform.position) < 20 && !fightStarted)
        {
            StartFight();
        }
            GetPlayerDirection();
        //When tagged as an enemy, the player can hurt the boss
        if (isVunerable)
        {
            this.gameObject.tag = "Enemy";

        }
        //When tagged as boss the player cannot harm the boss
        else
        {
            this.gameObject.tag = "Boss";
        }
    }
}
