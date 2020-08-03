# ChatController.
## Inherited from NetController

The Chat controller is used to send chat messages to all players.

## Public Variables

### `UnityEvent<string>` OnMessageReceived
Invoked when a message is received. Use this to update your chat window or server console.

## Public Methods

### `void` BroadcastMessage(`string` message)
Sends a message to everyone.

On the client the message is sent to the server, The server will process and format the message and send it back to the sender along with the rest of the players.

On the server, the message is sent to ALL players with out any modification.

## Basic Usage
To use the NetChat class your chat script will need to subscribe to the `OnMessageReceived` Action.
```c#
// ChatController is the networking namespace.
using PP.Networking;

// Subscribe to the OnMessageReceived action somewhere in
// your set-up or enable code, OnEnable is a good place.
private void OnEnable()
{
  NetChat.OnMessageReceived += IncomingMessage;
}

// You HAVE to un-subscribe or else null reference exceptions
// start to get thrown.
private void OnDisable()
{
  NetChat.OnMessageReceived += IncomingMessage;
}

// This gets called every time a chat message is received because we
// have subscribed to the OnMessageReceived event.
private void IncomingMessage(ChatMessage messagePacket)
{
  Debug.Log("Incoming Chat Message : " + messagePacket.message);
}
```