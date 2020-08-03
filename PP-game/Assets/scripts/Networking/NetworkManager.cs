using UnityEngine;
using System.Collections.Generic;

using PP.Utils;

namespace PP.Networking
{
  public class NetworkManager : MonoBehaviour
  {
    public Server.GameServer Server;
    public Client.GameClient Client;

    public EntityController entityController;

    public NetworkedEntity testPrefab;
    public void Awake()
    {
      var prefabs = new Dictionary<ushort, NetworkedEntity>();
      prefabs.Add(100, testPrefab);

      if(Application.isBatchMode)
      {
        Server.gameObject.AddComponent<ServerConsole>();
        Debug.Log("Starting as a Server");
        Destroy(Client);

        Server.Init();
        entityController.Setup(Server, prefabs);
        Server.Create();
      }
      else
      {
        Destroy(Server);
        Client.Init();        
        Client.Create();
        entityController.Setup(Client, prefabs);
        Client.Connect("localhost");
      }
    }

    private void Update()
    {
      if(Input.GetKeyDown(KeyCode.F1))
        NetChat.SendMessage("Hello World! This is a test message sent from a client");
    }
  }
}