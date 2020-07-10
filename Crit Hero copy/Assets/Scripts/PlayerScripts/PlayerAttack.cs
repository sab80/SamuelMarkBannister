using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public SpriteRenderer mySprite;
    public Animator anim;
    public GameObject playerObject;

    public int attackDamage;
    public int critDamage;
    public float critChance;
    private bool attacking;
    private float attackDuration;
    private float attackCD = 0.3f;
    public int currentAttackDamage;

    private int randCritGen;


    private bool checkIsFlipped;
    public GameObject RightCollider;
    public GameObject LeftCollider;

    
   
 

    private void Awake()
    {
        //Loading damage stats from playerController
        playerObject = GameObject.FindGameObjectWithTag("Player");
        mySprite = playerObject.GetComponent<SpriteRenderer>();
        anim = playerObject.GetComponent<Animator>();

        checkIsFlipped = mySprite.flipX;

        RightCollider = GameObject.Find("PlayerAttackTriggerRight");
        LeftCollider = GameObject.Find("PlayerAttackTriggerLeft");
        RightCollider.GetComponent<Collider2D>().enabled = false;
        LeftCollider.GetComponent<Collider2D>().enabled = false;
    }


    // Start is called before the first frame update
    void Start()
    { 
      
    }

    public void Attack()
    {
        if (Input.GetKeyDown("f") && !attacking)
        {
            randCritGen = Random.Range(0, 100);
            if (randCritGen < (critChance * 100))
            {
                Debug.Log("Crit");
                currentAttackDamage = critDamage;
                anim.SetBool("IsCritting", attacking);
            }
            else
            {
                currentAttackDamage = attackDamage;
            }
            
        
            
            attacking = true;
            attackDuration = attackCD;
            checkIsFlipped = mySprite.flipX;
            Debug.Log(checkIsFlipped);
            if (checkIsFlipped)
            {
                LeftCollider.GetComponent<Collider2D>().enabled = true;
            }
            else
            {
                RightCollider.GetComponent<Collider2D>().enabled = true;
            }

        }
        

        if (attacking)
        {
            if (attackDuration > 0)
            {
                attackDuration -= Time.deltaTime;

            }
            else
            {
                attacking = false;
                RightCollider.GetComponent<Collider2D>().enabled = false;
                LeftCollider.GetComponent<Collider2D>().enabled = false;
            }
        }
        anim.SetBool("IsAttacking", attacking);
        if (randCritGen < (critChance * 100))
        {
            anim.SetBool("IsCritting", attacking);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        attackDamage = GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().attackDamage;
        critDamage = GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().critDamage;
        critChance = GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().critChance;
        Attack();
    }
}
