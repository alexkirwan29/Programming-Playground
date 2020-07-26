using UnityEngine;
using System.Linq;

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
        Debug.Log("Starting as a Server");
        Destroy(Client);
        Server.StartNetworker();
      }
      else
      {
        Destroy(Server);
        Client.StartNetworker();
<<<<<<< HEAD
        Client.Connect("tomp.id.au");
=======
        Client.Connect("120.149.27.156");
>>>>>>> 965201523a502de6805b5643f2f2238192d9f223
      }
    }

    private void Update()
    {
      if(Input.GetKeyDown(KeyCode.F1))
        NetChat.SendMessage("Hello World! This is a test message sent from a client");
    }
  }
}