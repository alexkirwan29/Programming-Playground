using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

namespace PP.API.Endpoints
{
  public class Inventory : Endpoint
  {
    public Inventory(APIManager controller) : base(controller)
    {
      
    }

    public IEnumerator GetInventory(uint invId, UnityAction<Objects.Inventory> OnSuccess, UnityAction<Objects.Error> OnError = null)
    {
      yield return controller.MakeRequest("GET", $"/inventories/{invId}", this, OnSuccess, OnError);
    }
  }
}
