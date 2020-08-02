using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PP.Interact;

namespace PP
{
  public class PlayerInteractor : Entity
  {
    public Text InteractText;
    public float InteractDistance = 2.5f;
    public LayerMask InteractableLayers;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start was called!");
    }

    // Update is called once per frame.
    void Update()
    {
      // Create a ray from the camera's position pointing out in the direction of the cursor.
      var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      
      // Send the raycast out to the physics scene.
      RaycastHit hit;
      if(Physics.Raycast(ray, out hit, InteractDistance, InteractableLayers))
      {
        // Attempt to get an Interactable component.
        var interactable = hit.collider.gameObject.GetComponentInParent<Interactable>();
        if(interactable != null)
        {
          InteractText.text = interactable.InteractPrompt;
        }
      }
    }
  }
}