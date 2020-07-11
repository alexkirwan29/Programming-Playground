namespace PP.Networking.Server
{
  [System.Serializable]
  public class ServerSettings
  {
    public ushort ListenPort = 27015;
    public ushort MaxPlayers = 8;
    public string ServerName = "Untitled Server";

    public bool IPv6 = false;
  }
}