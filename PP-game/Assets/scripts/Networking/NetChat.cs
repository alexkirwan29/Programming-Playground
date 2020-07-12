// NetChat created by 45Ninjas aka Tom
// Documentation at scripts/Networking/Doc/chat.md

using UnityEngine;
using System;
using LiteNetLib;
using LiteNetLib.Utils;

namespace PP.Networking {
  public static class NetChat {

    public static bool IsReady {
      get {
        return controller.IsRunning;
      }
    }

    public class MessageFilterArgs : EventArgs {
      public string Message { get; set; }
      public bool StopMessage { get; set; }

      public MessageFilterArgs(string message) {
        Message = message;
        StopMessage = false;
      }
    }

    public static Action<MessageFilterArgs> OnFilterMessage;
    public static Action<string> OnMessageReceived;

    private static bool isServer;
    private static GameNetworker controller;



    public static void Initialise(GameNetworker controller) {
      // Subscribe to the ChatMessage packet.
      controller.packetProcessor.SubscribeReusable<Packets.ChatMessage>(MessageReceived);

      // Get a reference to the controller and set isServer if needed.
      NetChat.controller = controller;
      isServer = controller.GetType() == typeof(Server.GameServer);
    }

    public static void DeInitialise() {
      // Unsubscribe from events.
      controller.packetProcessor.RemoveSubscription<Packets.ChatMessage>();
    }


    public static void SendMessage(string message) {
      if (IsReady) {
        // Create the packet and send it it all peers.
        var packet = new Packets.ChatMessage(message);
        controller.netMan.SendToAll(controller.WritePacket(packet), DeliveryMethod.ReliableOrdered);
      } else {
        Debug.LogError($"{typeof(NetChat).FullName} isn't ready to send chat messages.");
      }
    }

    private static void MessageReceived(Packets.ChatMessage messagePacket) {
      // A message has been received.
      // If we are the server, filter the message and stop the message if any
      // filter asked us to. Finally Invoke OnMessageReceived.

      var args = new MessageFilterArgs(messagePacket.message);

      if (isServer) {
        // Invoke OnFilterMessage.
        if (OnFilterMessage != null)
          OnFilterMessage.Invoke(args);

        if (!args.StopMessage) {
          Debug.Log(args.Message);
          SendMessage(args.Message);
        }
      }

      // Invoke the OnMessageReceived action.
      if (OnMessageReceived != null && !args.StopMessage) {
        OnMessageReceived.Invoke(args.Message);
      }
    }
  }
}