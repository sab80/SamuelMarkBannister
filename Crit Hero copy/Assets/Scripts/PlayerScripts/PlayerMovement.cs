using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int moveSpeed = 8;
    public int jumpHeight = 5;
    public bool hasDoubleJumped = false;
    public bool onGround;
    private float horizontalMovement;
    public bool hasItemDoubleJump;

    public Rigidbody2D PlayerRigidbody;
    public GameObject GO;
    public Animator animator;
    public SpriteRenderer mySprite;
    public GameObject groundRay;
    public RaycastHit2D rayG;






    //public int playerHealth = 100;
    // Start is called before the first frame update

    void Start()
    {
		onGround = false;
        animator.SetBool("IsAlive", true);
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mySprite = this.gameObject.GetComponent<SpriteRenderer>();
        GO = GetComponent<GameObject>();
        
    }



    public void MovePlayer()
    {
        //PlayerRigidbody.velocity = new Vector2(horizontalMovement * moveSpeed, PlayerRigidbody.velocity.y);
        //PlayerRigidbody.MovePosition(transform.position + PlayerVector * moveSpeed * Time.deltaTime, PlayerRigidbody.velocity.y);
        //gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f, 0f), ForceMode2D.Impulse);
        //PlayerVector = Vector3.zero;
        horizontalMovement = Input.GetAxisRaw("Horizontal");

        if (horizontalMovement > 0 || horizontalMovement < 0)
        {
            animator.SetBool("Ismoving", true);
            PlayerRigidbody.velocity = new Vector2(horizontalMovement * moveSpeed, PlayerRigidbody.velocity.y);

            if (horizontalMovement < 0)
            {
                mySprite.flipX = true;
            }
            else
            {
                mySprite.flipX = false;
            }
        }
        else
        {
            PlayerRigidbody.velocity = new Vector2(0, PlayerRigidbody.velocity.y);
            animator.SetBool("Ismoving", false);
        }
    }


    //void OnCollisionEnter2D(Collision2D Collision)
    //{

    //    if (Collision.gameObject.tag == "Ground")
    //    {
    //        animator.SetBool("JumpPress", false);
    //        animator.SetBool("doubleJump", false);
    //        onGround = true;

    //    }
    //}

    //This script checks if the player is currently touching the ground
    public void CheckGrounded()
    {
        rayG = Physics2D.Raycast(groundRay.transform.position, new Vector3(0f, -0.05f, 0f), 0.2f);
        //Used to debug, to check where the raycast line is
        Debug.DrawRay(groundRay.transform.position, new Vector3(0f, -0.05f, 0f) * 0.8f);

        //Checks if the ray is initialised, as if it hits nothing it won't be
        if (rayG)
        {
            //Checks if grounded
            if (rayG.collider.CompareTag("Ground"))
            {
                onGround = true;
                animator.SetBool("JumpPress", false);
                animator.SetBool("doubleJump", false);

            }

            else
            {
                onGround = false;

            }
        }
                
 
    }

    //Gives the jump and double jump functionality
    void Jump()
    {
        //Double jump is checked for 
        if (Input.GetButtonDown("Jump") && onGround == false && hasDoubleJumped == false && hasItemDoubleJump)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 12f), ForceMode2D.Impulse);
            animator.SetBool("doubleJump", true);
            hasDoubleJumped = true;
        }
        //Jump is checked for
        if (Input.GetButtonDown("Jump") && onGround == true)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 12f), ForceMode2D.Impulse);
            animator.SetBool("JumpPress", true);
            onGround = false;
            hasDoubleJumped = false;
        }



    }
    //Disables playermovement while knocked back
    //This is not my code and is referenced in the report
    public void Disable(float time)
    {
        //This isnt my code.
        enabled = false;
        // If we were called multiple times, reset timer.
        CancelInvoke("Enable");
        // Note: Even if we have disabled the script, Invoke will still run.
        Invoke("Enable", time);
    }

    public void Enable()
    {
        enabled = true;
    }


    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        hasItemDoubleJump = GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().hasDoubleJump;
        MovePlayer();
        Jump();
    }

}