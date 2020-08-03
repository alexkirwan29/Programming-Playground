// NetChat created by 45Ninjas aka Tom
// Documentation at scripts/Networking/Doc/chat.md

using UnityEngine;
using UnityEngine.Events;
using System;
using LiteNetLib;
using LiteNetLib.Utils;

namespace PP.Networking {
  public class ChatController : MonoBehaviour, INetController {

    private Networker networker;
    public bool Ready => networker != null && networker.Running;

    // Setup the controller.
    public void Init(Networker networker) {
      this.networker = networker;
      networker.PacketProcessor.SubscribeReusable<Packets.ChatMessage>(MessageReceived);

      Debug.Log("Started", this);
    }

    // Shut down the controller.
    public void Shutdown() {
      networker.PacketProcessor.RemoveSubscription<Packets.ChatMessage>();
      networker = null;
      Debug.Log("Shutdown", this);
    }

    /// <summary>
    /// Broadcast a chat message to all clients or the server.
    /// </summary>
    /// <param name="message">The message to be sent.</param>
    public void BroadcastChatMessage(string message) {
      if (Ready) {
        var packet = new Packets.ChatMessage(message);
        networker.SendToAll(networker.WritePacket(packet), DeliveryMethod.ReliableOrdered);
      }
      else
        Debug.LogError($"{typeof(ChatController).FullName} isn't ready to send chat messages.");
    }

    public UnityEvent<MessageFilterArgs> OnFilterMessage;
    public UnityEvent<string> OnMessageReceived;

    public class MessageFilterArgs : EventArgs {
      public string Message { get; set; }
      public bool StopMessage { get; set; }

      public MessageFilterArgs(string message) {
        Message = message;
        StopMessage = false;
      }
    }

    private void MessageReceived(Packets.ChatMessage messagePacket) {
      // A message has been received.
      // If we are the server, filter the message and stop the message if any
      // filter asked us to. Finally Invoke OnMessageReceived.

      var args = new MessageFilterArgs(messagePacket.message);

      if (Networker.IsServer) {
        // Invoke OnFilterMessage.
        if (OnFilterMessage != null)
          OnFilterMessage.Invoke(args);

        if (!args.StopMessage) {
          BroadcastChatMessage(args.Message);
          Debug.Log(args.Message, this);
        }
      }

      // Invoke the OnMessageReceived action.
      if (OnMessageReceived != null && !args.StopMessage) {
        OnMessageReceived.Invoke(args.Message);
      }
    }
  }
}