using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Security.Policy;

public class interactWithItemText : MonoBehaviour
{

    public int maxDistance = 10;
    
    [SerializeField] TextMeshProUGUI interactLabel; //text that shows on screen asking if you want to interact
    bool lookingAtIem = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance) &&
        hit.collider.gameObject.CompareTag("interactable"))
        {
            interactLabel.text = "item hit";
        }


    }
}
