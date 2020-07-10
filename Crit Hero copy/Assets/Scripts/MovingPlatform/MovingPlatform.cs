using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D platfromRB;
    private float startPositionX;
    private float startPositionY;
    private float topOfMovement;
    private bool goingUp;

    public int platformSpeed = 200;
    public int platformDistance = 12;
    void Start()
    {
        startPositionX = this.transform.position.x;
        startPositionY = this.transform.position.y;
        topOfMovement = startPositionY + platformDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (goingUp && this.transform.position.y < topOfMovement)
        {
            platfromRB.velocity = new Vector2(0, platformSpeed * Time.deltaTime);
            
        }
        else if(this.transform.position.y >= topOfMovement)
        {
            goingUp = false;
            
        }

        if (!goingUp && this.transform.position.y > startPositionY)
        {
            platfromRB.velocity = new Vector2(0, -platformSpeed * Time.deltaTime);
        }
        else
        {
            goingUp = true;
        }
       
      
    }
}
