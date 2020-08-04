using System;
using LiteNetLib.Utils;

namespace PP.Networking {
  public class JoinRequest : INetSerializable {
    const int VERSION_LIMIT = 12;
    const int USERNAME_LIMIT = 36;
    const int PASSWORD_LIMIT = USERNAME_LIMIT;
    const int SKINURL_LIMIT = 128;
    const int MAX_SIZE = VERSION_LIMIT + USERNAME_LIMIT + PASSWORD_LIMIT + SKINURL_LIMIT + (sizeof(int) * 4);

    public string GameVersion;
    public string Username;
    public string Password;
    public string SkinUrl;

    public JoinRequest() {
      Username = System.Environment.UserName;
      Password = null;
      SkinUrl = null;
      GameVersion = Networker.GAME_VERSION;
    }

    public void Serialize(NetDataWriter writer) {
      writer.Put(Networker.GAME_VERSION);
      writer.Put(Password);
      writer.Put(Username);
      writer.Put(SkinUrl);
    }

    public void Deserialize(NetDataReader reader) {
      GameVersion = reader.GetString(VERSION_LIMIT);
      Password = reader.GetString(PASSWORD_LIMIT);
      Username = reader.GetString(USERNAME_LIMIT);
      SkinUrl = reader.GetString(SKINURL_LIMIT);

    }

    static public bool CheckRequest(NetDataReader reader, string password, out string reason, out JoinRequest request) {
      request = new JoinRequest();

      // Make sure the 
      if (reader.AvailableBytes > MAX_SIZE) {
        reason = "Join request too large";
        return false;
      }

      if (!reader.TryGetString(out request.GameVersion)) {
        reason = $"Game Version Incorrect. Server Version: {Networker.GAME_VERSION}";
        return false;
      }

      if (request.GameVersion != Networker.GAME_VERSION) {
        reason = $"Game Version Incorrect. Server Version: '{Networker.GAME_VERSION}', Your Version: '{request.GameVersion}'";
        return false;
      }

      if (
        !reader.TryGetString(out request.Password) ||
        !reader.TryGetString(out request.Username) ||
        !reader.TryGetString(out request.SkinUrl)) {
        reason = "Invalid Join Request";
        return false;
      }

      if (string.IsNullOrEmpty(password) && string.IsNullOrEmpty(request.Password) || password == request.Password) {
        reason = "Congrats, You're not an illegal alien!";
        return true;
      }
      else
        reason = "Incorrect Password";
      return false;
    }
  }
}