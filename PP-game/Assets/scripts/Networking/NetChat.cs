using System;

namespace PP.Networking
{
  public static class NetChat
  {
    // Allow other code to know if the controller is running.
    public static bool IsReady
    { get {
      return controller != null && controller.IsRunning;
    }}

    public static Action<Packets.ChatMessage> OnMessageReceived;

    // The controller that this Class interacts with.
    private static GameNetworker controller;

    public static void Initialise(GameNetworker controller)
    {
      OnMessageReceived += debugChatMessage;
      // Subscribe to the ChatMessage packet.
      controller.packetProcessor.SubscribeReusable<Packets.ChatMessage>(OnMessageReceived);

      // Get a reference to the controller.
      NetChat.controller = controller;
    }

    private static void debugChatMessage(Packets.ChatMessage messagePacket)
    {
      UnityEngine.Debug.Log("Chat Message: " + messagePacket.message);
    }

    public static void DeInitialise()
    {
      NetChat.controller.packetProcessor.RemoveSubscription<Packets.ChatMessage>();
      NetChat.controller = null;
    }

    /// <summary>
    /// Sends a chat message to all players.
    /// </summary>
    /// <param name="message"></param>
    public static void SendMessage(string message)
    {
      // Only send the chat message packet if the controller is ready.
      if(IsReady)
        controller.SendPacket(new Packets.ChatMessage(message));
      
      // Write a sort of detailed message when something went wrong.
      else
        UnityEngine.Debug.LogError
        (
          $"{typeof(NetChat).FullName} is not in a state where chat messages can be sent. IsReady:{IsReady}"
        );
    }
  }
}