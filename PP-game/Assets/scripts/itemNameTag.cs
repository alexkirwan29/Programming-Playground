using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class itemNameTag : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI item_id;
    [SerializeField] TextMeshProUGUI item_name;
    [SerializeField] TextMeshProUGUI item_value;

    // Start is called before the first frame update
    void Start()
    {

        item_id.text    = "289";     
        item_name.text  = "blue_block";     
        item_value.text = "$0";     

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
