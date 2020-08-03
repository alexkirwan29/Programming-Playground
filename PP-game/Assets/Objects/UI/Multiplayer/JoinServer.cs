using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PP.Networking;

namespace PP.UI {
  public class JoinServer : MonoBehaviour {
    public InputField username;
    public InputField skinUrl;
    public InputField serverAddress;
    public InputField serverPassword;
    public Text message;

    public PP.Networking.Client.GameClient gameClient;

    private void Awake() {
      username.text = PlayerPrefs.GetString("pp.ui.join-server.username", "[ No Name ]");
      skinUrl.text = PlayerPrefs.GetString("pp.ui.join-server.skin-url", null);
      serverAddress.text = PlayerPrefs.GetString("pp.ui.join-server.server-address", "localhost");
      serverPassword.text = PlayerPrefs.GetString("pp.ui.join-server.server-password", null);
    }

    public void SetMessage(LiteNetLib.DisconnectReason reason, string message) {
      if(message != null)
        SetMessage($"You have been disconnected. Reason: {message}");
      else
        SetMessage($"You have been disconnected. Reason: {reason}");
    }

    public void SetMessage(string message) {
      this.message.text = message;
    }

    public void JoinButton() {
      var joinRequest = new JoinRequest();

      if (!string.IsNullOrWhiteSpace(username.text)) {
        joinRequest.Username = username.text;
      }

      if (!string.IsNullOrWhiteSpace(skinUrl.text)) {
        joinRequest.SkinUrl = skinUrl.text;
      }

      if (!string.IsNullOrWhiteSpace(serverPassword.text)) {
        joinRequest.Password = serverPassword.text;
      }

      if (!string.IsNullOrWhiteSpace(serverAddress.text)) {
        SetMessage("Connecting...");
        gameClient.Connect(serverAddress.text, joinRequest);

        PlayerPrefs.SetString("pp.ui.join-server.username", username.text);
        PlayerPrefs.SetString("pp.ui.join-server.skin-url", skinUrl.text);
        PlayerPrefs.SetString("pp.ui.join-server.server-address", serverAddress.text);
        PlayerPrefs.SetString("pp.ui.join-server.server-password", serverPassword.text);
      }
    }
  }

}