# NetChat

Used for clients and servers to send chat messages to ALL players.

If the server is sending messages it should format the text for end user consumption before calling `SendMessage`. (e.g `[put-username]: in the start of the message`)

## Public Variables

### `bool` IsReady (readonly)
Let's you know when the NetChat is ready.

Returns true when the underlying network controller (the one that's set in `Initialise`) is Running.

### `Action<Packets.ChatMessage>` OnMessageReceived
Is an action other scripts can listen to to know when a chat message has been received.

## Public Methods

### `void` SendMessage(`string` message)
Sends a message to everyone.

On the client the message is sent to the server, The server will process and format the message and send it back to the sender along with the rest of the players.

On the server, the message is sent to ALL players with out any modification.

### `void` Initialise(`GameNetworker` controller)
Initialises the NetChat class with the specified controller.

This is where the NetChat subscribes to the packet processor.

This should be invoked by the client or server once it's created before connections have been started.

### `void` DeInitialise()
Undoes what Initialise did.

Un-subscribes from the packet processor and Un-Initialises the NetChat class.

This should be invoked by the client or server once it's closed all connections before shutting down.


## Basic Usage
To use the NetChat class your chat script will need to subscribe to the `OnMessageReceived` Action.
```c#
// This is the namespace of ChatMessage.
using PP.Networking.Packets;

// Subscribe to the OnMessageReceived action somewhere in
// your set-up or enable code, OnEnable is a good place.
private void OnEnable()
{
  NetChat.OnMessageReceived += incomingMessage;
}

// You HAVE to un-subscribe or else null reference exceptions
// start to get thrown.
private void OnDisable()
{
  NetChat.OnMessageReceived += incomingMessage;
}

// This gets called every time a chat message is received because we
// have subscribed to the OnMessageReceived event.
private void incomingMessage(ChatMessage messagePacket)
{
  Debug.Log("Incoming Chat Message : " + messagePacket.message);
}
```