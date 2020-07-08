using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PP.Interact
{
  public class Interactable : MonoBehaviour
  {
    public UnityAction<Entity> OnInteract;

    // The text that should appear when the user can interact with this.
    public string InteractPrompt;

    /// <summary>
    /// Invoke the OnInteract event when this Interactable has been interacted with.
    /// </summary>
    /// <param name="entity">The Entity that is interacting with this intractable.</param>
    public void Interact(Entity entity)
    {

      // Verify that the OnInteract 
      if(OnInteract != null)
        OnInteract.Invoke(entity);
    }
  }
}