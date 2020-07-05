using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

using Newtonsoft.Json;

namespace PP.API
{
  public class Controller : MonoBehaviour
  {
    public string url = "http://api.pp.nrms.xyz";

    /// <summary>
    /// Makes a request to the Database invoking OnSuccess OR OnError.
    /// </summary>
    /// <param name="method">The HTTP method to use (GET, POST, PUT, DELETE, UPDATE).</param>
    /// <param name="url">The URL prefix to use.</param>
    /// <param name="sender">The object making the request.</param>
    /// <param name="OnSuccess">The Action that's called when the data was successfully obtained</param>
    /// <param name="OnError">The Action that's called when the api or network returned an error.</param>
    /// <typeparam name="T">The type of object to get from the database.</typeparam>
    /// <returns></returns>
    public IEnumerator MakeRequest<T> (string method, string url, Endpoint sender, UnityAction<T> OnSuccess, UnityAction<Objects.Error> OnError)
    {
      // Make the request object.
      var request = new UnityWebRequest();

      // Set the URL, Method, timeout and downloadHandler.
      request.url = this.url + url;
      request.method = method;
      request.timeout = 20;
      request.downloadHandler = new DownloadHandlerBuffer();

      // Set the requested content type. (API dose not care, will always return json)
      request.SetRequestHeader("Content-Type", "application/json");

      // Wait until the request has been processed by the API.
      yield return request.SendWebRequest();

      // Check to see if there is an error.
      if(request.isNetworkError || request.isHttpError)
      {
        // Create the error object.
        var error = new Objects.Error(request.url, request.responseCode, request.error);

        // Log the error to the console.
        Debug.LogError($"API Error [{sender.GetType()}]: {error}");

        // Invoke OnError if it's not null.
        if(OnError != null)
          OnError.Invoke(error);
      }

      else
      {
        // Looks like everything worked. Wooh!
        // Deserialize the JSON data into whatever the generic type is.
        T obj = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);

        // Invoke the OnSuccess action.
        OnSuccess.Invoke(obj);
      }
    }
  }
}



// The following are examples on how to use the API.

// GET PLAYERS.

//   var players = new Endpoints.Players(this);

//   // Print the size of the array of players downloaded from the API.
//   StartCoroutine(players.GetAllPlayers
//   (
//     (Objects.Player[] allPlayers) =>
//     {
//       Debug.Log(allPlayers.Length);
//     }
//   ));

//   // Print the name of the player with ID of 1.
//   StartCoroutine(players.GetPlayer
//   (
//     1,
//     (Objects.Player player ) =>
//     {
//       Debug.Log(player.UserName);
//     }
//   ));



// GET ITEMS

//   var items = new Endpoints.Items(this);

//   //Print the name and description of an item with the ID of 5
//   StartCoroutine(items.GetItem
//   (
//     5,
//     (Objects.Item item ) =>
//     {
//       Debug.Log(item.Name);
//       Debug.Log(item.Description);
//     }
//   ));

//   // Print the name and description of an item with the ID of 5
//   StartCoroutine(items.GetAllItems
//   (
//     (Objects.Item[] allItems ) =>
//     {
//       Debug.Log(allItems.Length);
//     }
//   ));



// GET INVENTORIES

//   var inventories = new Endpoints.Inventory(this);

//   // Print the name of an inventory along with the amount of inventory slots taken up.
//   StartCoroutine(inventories.GetInventory
//   (
//     1,
//     (Objects.Inventory inventory ) =>
//     {
//       Debug.Log(inventory.Name);
//       Debug.Log(inventory.Slots.Length);
//     }
//   ));
// }