using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continue : MonoBehaviour
{
    //This is not my code, reference in report
    public static Continue Instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //Variables to be passed through scenes
    public bool isLoadNeeded;
    public bool isnewGameNeeded;

    // Update is called once per frame
    void Update()
    {
        
    }
    //This destroys any other instances of the class so theres only ever one
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
