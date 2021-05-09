using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoad : MonoBehaviour
{
   

    //The path of the Game save file to read and write to
    string path = "Assets/Resources/GameSave.txt";
    //variable that will be given a from the continueScript
    public bool isLoadNeeded;
    public Continue continueScript;
    

    // Start is called before the first frame update
    void awake()
    {
  
    }

    private void Start()
    {

    }



    public void LoadFromFile()
    {
        Debug.Log("LOADING...");

        if (File.Exists(path))
        {
            StreamReader inp_stm = new StreamReader(path);

            while (!inp_stm.EndOfStream)
            {
                //variables used to help assign the string read in to the correct variable in globalController
                string inp_ln = inp_stm.ReadLine();
                int i = 0;
                //Counts the breaks found in the text file
                int breakCounter = 0;
                char[] currentStringArr;
                currentStringArr = new char[21];
                String currentString;
               
                int currentCharPos = 0;
                while (inp_ln[i] != '^')
                {
                    Debug.Log(inp_ln[i]);
                    if(inp_ln[i] == '|')
                    {
                        Debug.Log("FoundOne");
                        
                        currentString = new string(currentStringArr);

                        for (int j = 0; j < currentStringArr.Length; j++)
                        {
                            currentStringArr[j] = '\0';
                        }   
                        switch (breakCounter)
                        {
                            case 0:
                                Debug.Log("Case0 stringPrint");
                                Debug.Log(currentString);
                                int.TryParse(currentString, out GlobalControl.Instance.gold);
                                currentCharPos = 0;
                                breakCounter++;
                                i++;
                                break;
                            case 1:
                                int.TryParse(currentString, out GlobalControl.Instance.health);
                                currentCharPos = 0;
                                breakCounter++;
                                i++;
                                break;
                            case 2:
                                int.TryParse(currentString, out GlobalControl.Instance.maxHealth);
                                currentCharPos = 0;
                                breakCounter++;
                                i++;
                                break;
                            case 3:
                                Boolean.TryParse(currentString, out GlobalControl.Instance.hasDoubleJump);
                                currentCharPos = 0;
                                breakCounter++;
                                i++;
                                break;
                            case 4:
                                Boolean.TryParse(currentString, out GlobalControl.Instance.hasMap);
                                currentCharPos = 0;
                                breakCounter++;
                                i++;
                                break;
                            case 5:
                                int.TryParse(currentString, out GlobalControl.Instance.attackDamage);
                                currentCharPos = 0;
                                breakCounter++;
                                i++;
                                break;
                            case 6:
                                int.TryParse(currentString, out GlobalControl.Instance.critDamage);
                                currentCharPos = 0;
                                breakCounter++;
                                i++;
                                break;
                            case 7:
                                float.TryParse(currentString, out GlobalControl.Instance.critChance);
                                currentCharPos = 0;
                                breakCounter++;
                                i++;
                                break;
                            case 8:
                                GlobalControl.Instance.currentSpawnPoint = StringToVector(currentString);
                                currentCharPos = 0;
                                breakCounter++;
                                i++;
                                break;
                            case 9:
                                GlobalControl.Instance.currentPlayerPosition = StringToVector(currentString);
                                currentCharPos = 0;
                                breakCounter++;
                                i++;
                                break;
                            case 10:
                                Boolean.TryParse(currentString, out GlobalControl.Instance.ShopTreeActive);
                                currentCharPos = 0;
                                breakCounter++;
                                i++;
                                break;
                            case 11:
                                Boolean.TryParse(currentString, out GlobalControl.Instance.GreenTreeActive);
                                currentCharPos = 0;
                                breakCounter++;
                                i++;
                                break;
                            case 12:
                                Boolean.TryParse(currentString, out GlobalControl.Instance.BlueTreeActive);
                                currentCharPos = 0;
                                breakCounter++;
                                i++;
                                break;
                            case 13:
                                Boolean.TryParse(currentString, out GlobalControl.Instance.BossTreeActive);
                                currentCharPos = 0;
                                breakCounter++; 
                                i++;
								break;
							case 14:
								int.TryParse(currentString, out GlobalControl.Instance.upgradeCounter);
								currentCharPos = 0;
								breakCounter++;
								i++;
								break;
                        }
                      
                    }
                    else
                    {
                        
                        currentStringArr[currentCharPos] = inp_ln[i];
                        currentCharPos++;
                        i++;
       
                    }
                }
                
            }
             
            inp_stm.Close();
        }
        //This tells the playercontroller to re-update all of its attributes.
        GlobalControl.Instance.GetToString();
        GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().LoadFromGlobal();
    }


   public void SaveToFile()
    {
        Debug.Log("SAVING...");
        
        File.WriteAllText(path, String.Empty);
        //Make sure Global Controller is up to date.
        GameObject.Find("PlayerGameObject").GetComponent<PlayerController>().SavePlayerStats();
        //Calls toString to get the data needed to save.
        string saveTostring = GlobalControl.Instance.GetToString();

        
        if (File.Exists(path))
        {
            Debug.Log(path + " already exists.");
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(saveTostring);

            Debug.Log(saveTostring);
            writer.Close();
            Debug.Log("Finished Writing");
        }
        else { Debug.Log("File doesn't exist"); }
    }

    //This method is used to convert string to vector3 for when the file is loaded
    public Vector3 StringToVector(String thestring)
    {
        thestring = thestring.Substring(1, thestring.Length - 2);
        string[] splitArray = thestring.Split(',');
        float x; float.TryParse(splitArray[0], out x);
        float y; float.TryParse(splitArray[1], out y);
        float z; float.TryParse(splitArray[2], out z);
        Vector3 theVector = new Vector3(x, y, z);
        return theVector;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

   
}
