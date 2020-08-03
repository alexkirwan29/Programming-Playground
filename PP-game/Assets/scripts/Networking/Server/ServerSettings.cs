namespace PP.Networking.Server
{
  [System.Serializable]
  public class ServerSettings
  {
    public ushort ListenPort = 27015;
    public ushort MaxPlayers = 8;
    public string Name = "Untitled Server";
    public string Password = null;

    public bool IPv6 = false;
  }
}