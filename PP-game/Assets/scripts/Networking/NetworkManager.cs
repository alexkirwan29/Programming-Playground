using UnityEngine;
using System.Collections.Generic;

using PP.Utils;

namespace PP.Networking
{
  public class NetworkManager : MonoBehaviour
  {
    public Server.GameServer Server;
    public Client.GameClient Client;

    public void Awake()
    {
      if(Application.isBatchMode)
      {
        Server.gameObject.AddComponent<ServerConsole>();
        Destroy(Client);
        Debug.Log("Starting as a Server");
        Server.Init();
      }
      else
      {
        Destroy(Server);
        Client.Init();        
        Client.Connect("localhost");
      }
    }

    private void Update()
    {
      if(Input.GetKeyDown(KeyCode.F1))
        Client.Chat.BroadcastChatMessage("Hello World! This is a test message sent from a client");
    }
  }
}