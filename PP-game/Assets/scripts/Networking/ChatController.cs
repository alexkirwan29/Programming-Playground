// NetChat created by 45Ninjas aka Tom
// Documentation at scripts/Networking/Doc/chat.md

using UnityEngine;
using UnityEngine.Events;
using LiteNetLib;

namespace PP.Networking {
  public class ChatController : NetController {
    public const byte CONTROLLER_ID = 15;
    public override byte GetId() => CONTROLLER_ID;

    /// <summary>
    /// Broadcast a chat message to all clients or the server.
    /// </summary>
    /// <param name="message">The message to be sent.</param>
    public void BroadcastChatMessage(string message) {
      var writer = net.GetWriter(this);
      writer.Put(message);
      net.SendToAll(writer, DeliveryMethod.ReliableOrdered);
      Debug.Log(message, this);
    }

    public UnityEvent<string> OnMessageReceived;

    public override void Read(NetPacketReader reader, NetPeer peer) {
      string message = reader.GetString();
      if (Networker.IsServer) {
        // TODO: Format the message to show the username of the sender.
        BroadcastChatMessage(message);
      }

      if (OnMessageReceived != null)
        OnMessageReceived.Invoke(message);

      base.Read(reader, peer);
    }
  }
}