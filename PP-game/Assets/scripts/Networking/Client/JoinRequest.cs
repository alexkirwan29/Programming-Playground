using System;

namespace PP.Networking.Client
{
    public class JoinRequest
    {
      public string Username {get; set;}
      public string Password {get; set;}
      public string SkinUrl {get; set;}

      public JoinRequest()
      {
        Username = System.Environment.UserName;
        Password = null;
        SkinUrl = null;
      }
    }
}
