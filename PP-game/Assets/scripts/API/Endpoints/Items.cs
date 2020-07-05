using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PP.API.Endpoints
{
  public class Items : Endpoint
  {
    public Items(Controller controller) : base(controller)
    {

    }

    public IEnumerator GetItem(uint itemId, UnityAction<Objects.Item> OnSuccess, UnityAction<Objects.Error> OnError = null)
    {
      yield return controller.MakeRequest("GET", $"/items/{itemId}", this, OnSuccess, OnError);
    }

    public IEnumerator GetAllItems(UnityAction<Objects.Item[]> OnSuccess, UnityAction<Objects.Error> OnError = null)
    {
      yield return controller.MakeRequest("GET", "/items/", this, OnSuccess, OnError);
    }
  }
}
