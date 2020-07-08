using UnityEngine;

namespace PP.Networking
{
  public class NetworkManager : MonoBehaviour
  {
    public void Start()
    {
      #if PP_SERVER
      startServer();
      #else
      startCient();
      #endif
    }

    private void startServer()
    {

    }
    private void startCient()
    {

    }
  }
}