using UnityEngine;
using System.Linq;

using PP.Utils;

namespace PP.Networking
{
  public class NetworkManager : MonoBehaviour
  {
    public Server.GameServer Server;
    public Client.GameClient Client;

    private StdLogger logger = new StdLogger();

    public void Start()
    {
      if(Application.isBatchMode)
      {
        StdLogger.MakeDefaultLogger(true);
        Debug.Log("Starting as a Server");
        Destroy(Client);
        Server.StartNetworker();
      }
      else
      {
        Destroy(Server);
        Client.StartNetworker();
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