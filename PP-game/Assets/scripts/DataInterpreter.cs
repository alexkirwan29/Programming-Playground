using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataInterpreter : MonoBehaviour
{

    //DISCLAIMER: Please treat this code as EXTREME TEMPORAY, 
    //I am only trying to simulate funtion so I can make a UI
    //It is completly nesscary for all code to be redone 

    public playerData playerData;
    public Text player_ID;
    public Text Username;
    public Text lastLogin;


    // Start is called before the first frame update
    void Start()
    {
              
        
        player_ID.text = playerData.player_ID.ToString();
        Username.text = playerData.username;
        lastLogin.text = playerData.last_login;

    }

}
