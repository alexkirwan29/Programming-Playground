using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LiteNetLib;
using LiteNetLib.Utils;
using PP.Networking;
using System.Net;
using System.Net.Sockets;

namespace PP.Networking.Client
{
  public class GameClient : Networker, INetEventListener
  {
    internal override void Create()
    {
      if(Client != null)
        throw new System.Exception("Only one client can be running");

      netMan = new NetManager(this)
      {
        AutoRecycle = true,
      };

      netMan.Start();

      IsClient = true;
      Client = this;
    }
    internal override void Destroy()
    {
      if(netMan != null && netMan.IsRunning)
        netMan.Stop(true);

      IsClient = false;
      Client = null;
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
      throw new System.NotImplementedException();
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
      Debug.LogError($"Network Error: {socketError}", this);
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
      // TODO: Show this to the end user somehow.
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {
      // Read the packets using the packet processor.
      packetProcessor.ReadAllPackets(reader);
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
      throw new System.NotImplementedException();
    }

    public void OnPeerConnected(NetPeer peer)
    {
      Debug.Log("you have joined the game");
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
      Debug.Log("you have been disconnected from the game");
    }

        public void Connect(string connectionString)
    {
      // Complain if the string is empty.
      if(string.IsNullOrWhiteSpace(connectionString))
        throw new System.NullReferenceException("Connection string is null or empty.");

      // Split the string on every colon.
      var parts = connectionString.Split(':');

      // Throw an error if the amount of strings returned are bigger than a host and port.
      if(parts.Length > 2)
        throw new ParseException("Malformed connection string, too many colons ( : )");

      // Set the host.
      string host = parts[0];

      // Looks like we might have a port.
      if(parts.Length == 2)
      {
        // Try and parse the port.
        ushort parsedPort;
        if(ushort.TryParse(parts[1], out parsedPort))
        {
          // Cool let's attempt to connect to it.
          Connect(host, parsedPort);
        }
      
        // Something went wrong there.
        else
          throw new ParseException("Invalid port after colon");
      }

      // TODO: Look up DNS SRV records of the domain name and use that as the port.

      // Ahhh let's try it anyways with the default port. See what happens.
      Connect(host, 27015);

    }

    public void Connect(string host, ushort port)
    {
      Debug.Log($"Attempting connection to {host} on port {port}.");
      netMan.Connect(host, port, "hello");
    }
  }
}
