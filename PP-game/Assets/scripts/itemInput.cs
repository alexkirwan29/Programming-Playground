using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemInput : MonoBehaviour
{

    public InputField itemName;  //human readable name i.e. Pork Chop
    public InputField itemDescription; //human readable desc. i.e. Meat from a pig
    public Button gotoIconEditor; //opens up icon editor - to be made
    public Button submit;//gathers all data entries and writes to ppdb
    public GameObject inputPanel; //the panel that contains all of this stuff - added here so there can be a button to toggle it on or off
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

      

    }
}
