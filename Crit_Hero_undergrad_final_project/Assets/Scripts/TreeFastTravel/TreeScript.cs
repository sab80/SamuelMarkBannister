    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{

    public Animation anim;
    public Animator animator;
    public bool treeActive;
    public GameObject FastTravelUI;
    public TravelTreeUi travelTreeScript;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        if(GameObject.Find("FastTravelUi") != null) { FastTravelUI = GameObject.Find("FastTravelUi"); }
        
        travelTreeScript = FastTravelUI.GetComponent<TravelTreeUi>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When the player attacks the tree the Travel map is opened
        if (collision.CompareTag("PlayerHitBox") && treeActive == true)
        {
            travelTreeScript.ActivateTravelMap();  
        }
        else
        {
            //If the player walks into the tree the spawn is update, the tree is active and the player is full healed
            Activate();
            
            //Set to active spawn point!
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().SendMessageUpwards("FullHeal");
                GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().currentSpawnPoint = this.gameObject.transform.position;

                collision.GetComponent<PlayerController>().SendMessageUpwards("AddActiveTree", this.gameObject);
            }
        }
    }

    //called to activate a tree
    public void Activate()
    {
        anim.Play("treeAnimations");
        animator.SetBool("IsActive", true);
        treeActive = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
