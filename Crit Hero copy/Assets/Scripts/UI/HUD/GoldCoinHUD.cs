using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoinHUD : MonoBehaviour
{
    public GameObject PlayerObject;
    public Animator healthAnim;


    // Start is called before the first frame update
    private void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        
    }

    public void PositionCoinHUD()
    {
        //Positions the gold to the top left of the camera
        this.transform.position = new Vector3(PlayerObject.transform.position.x - 5, PlayerObject.transform.position.y + 5.5f, 0);
    }

    public void Update()
    {
        PositionCoinHUD();
    }

}
