using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    public GameObject map;
    private bool isShowing = false;
    public bool hasMap;
    // Start is called before the first frame update
    void Start()
    {

    }
    void Awake()
    {
        map = GameObject.Find("Map");  
    }

    // Update is called once per frame
    void Update()
    {
        //If the player has the map item, then by pressing m the minimap is displayed
        hasMap = GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().hasMap;
        if (Input.GetKeyDown("m") && hasMap)
        {
            isShowing = !isShowing;
        }

        if (isShowing)
        {
            map.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!isShowing)
        {
            map.SetActive(false);
            Time.timeScale = 1;

        }

    }
} 
 