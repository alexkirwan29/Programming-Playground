using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace PP.API.Endpoints
{
  public class Players : Endpoint
  {
    public Players(Controller controller) : base(controller)
    {

    }

    public IEnumerator GetPlayer(uint playerId, UnityAction<Objects.Player> OnSuccess, UnityAction<Objects.Error> OnError = null)
    {
      yield return controller.MakeRequest("GET", $"/players/{playerId}", this, OnSuccess, OnError);
    }

    public IEnumerator GetAllPlayers(UnityAction<Objects.Player[]> OnSuccess, UnityAction<Objects.Error> OnError = null)
    {
      yield return controller.MakeRequest("GET", "/players/", this, OnSuccess, OnError);
    }
  }
}
