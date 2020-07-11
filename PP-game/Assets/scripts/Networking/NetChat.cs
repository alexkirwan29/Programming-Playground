// NetChat created by 45Ninjas aka Tom
// Documentation at scripts/Networking/Doc/chat.md

using UnityEngine;
using System;
using LiteNetLib;
using LiteNetLib.Utils;

namespace PP.Networking
{
  public static class NetChat
  {
    // Allow other code to know if the controller is running.
    public static bool IsReady
    {
      get
      {
        return controller.IsRunning;
      }
    }

    private static bool isServer;

    // The action that is called when a chat message has been received.
    public static Action<string> OnMessageReceived;

    // The controller that this Class interacts with.
    private static GameNetworker controller;

    // Initialises and subscribes to events.
    public static void Initialise(GameNetworker controller)
    {
      // Subscribe to the ChatMessage packet.
      controller.packetProcessor.SubscribeReusable<Packets.ChatMessage>(messageReceived);


      // Get a reference to the controller.
      NetChat.controller = controller;

      isServer = controller.GetType() == typeof(Server.GameServer);
    }

    public static void DeInitialise()
    {
      NetChat.controller.packetProcessor.RemoveSubscription<Packets.ChatMessage>();
    }

    /// <summary>
    /// Sends a chat message to all players.
    /// </summary>
    /// <param name="message"></param>
    public static void SendMessage(string message)
    {
      // Only send the chat message packet if the controller is ready.
      if(IsReady)
      {
        controller.netMan.SendToAll(controller.WritePacket(new Packets.ChatMessage(message)), DeliveryMethod.ReliableOrdered);
      }
      
      // Write a sort of detailed message when something went wrong.
      else
        Debug.LogError
        (
          $"{typeof(NetChat).FullName} is not in a state where chat messages can be sent. IsReady:{IsReady}"
        );
    }
    
    private static void messageReceived(Packets.ChatMessage messagePacket)
    {
      string msg = messagePacket.message;

      if(isServer)
      {
        // Debug.Log(msg);
        SendMessage(msg);
      }

      Debug.Log(msg);

      if(OnMessageReceived != null)
        OnMessageReceived.Invoke(msg);
    }
  }
}