using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

using PP.API.Endpoints;
using PP.API.Objects;

using Newtonsoft.Json;

namespace PP.API
{
  public class Controller : MonoBehaviour
  {
    public string url = "http://api.pp.nrms.xyz";

    public IEnumerator MakeRequest<T> (string method, string url, Endpoint sender, UnityAction<T> OnSuccess, UnityAction<Objects.Error> OnError)
    {
      var request = new UnityWebRequest();
      var dlh = new DownloadHandlerBuffer();
      // Setup the request.
      request.url = this.url + url;
      request.method = method;
      
      request.timeout = 20;
      request.redirectLimit = 5;

      request.useHttpContinue = false;
      request.downloadHandler = dlh;
      request.SetRequestHeader("Content-Type", "application/json");

      // Send the request.
      yield return request.SendWebRequest();

      // If there was an error, log it and invoke OnError if it exits.
      var error = new Objects.Error(request.url, request.responseCode, request.error);
      if(request.isNetworkError || request.isHttpError)
      {
        LogError(sender, error.ToString());
        if(OnError != null)
          OnError.Invoke(error);
      }
      else
      {
        T obj = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
        OnSuccess.Invoke(obj);
      }
    }
    public void LogError(Endpoint e, string text)
    {
      Debug.LogError($"Network Error [{e.GetType()}]: {text}");
    }

    public void Start()
    {
      var players = new Endpoints.Players(this);
      var items = new Endpoints.Items(this);
      var inventories = new Endpoints.Inventory(this);

      // Print the size of the array of players downloaded from the API.
      StartCoroutine(players.GetAllPlayers
      (
        (Objects.Player[] allPlayers) =>
        {
          Debug.Log(allPlayers.Length);
        }
      ));

      // Print the name of the player with ID of 1.
      StartCoroutine(players.GetPlayer
      (
        1,
        (Objects.Player player ) =>
        {
          Debug.Log(player.UserName);
        }
      ));

      // Print the name and description of an item with the ID of 5
      StartCoroutine(items.GetItem
      (
        5,
        (Objects.Item item ) =>
        {
          Debug.Log(item.Name);
          Debug.Log(item.Description);
        }
      ));

      // Print the name and description of an item with the ID of 5
      StartCoroutine(items.GetAllItems
      (
        (Objects.Item[] allItems ) =>
        {
          Debug.Log(allItems.Length);
        }
      ));

    
      // Print the name of an inventory along with the amount of inventory slots taken up.
      StartCoroutine(inventories.GetInventory
      (
        1,
        (Objects.Inventory inventory ) =>
        {
          Debug.Log(inventory.Name);
          Debug.Log(inventory.Slots.Length);
        }
      ));
    }
  }
}