using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LiteNetLib;
using LiteNetLib.Utils;
using PP.Networking;
using System.Net;
using System.Net.Sockets;

namespace PP.Networking.Server {
  public class GameServer : Networker, INetEventListener {
    public ServerSettings settings;

    internal override void Create() {
      if (Server != null)
        throw new System.Exception("Only one server can at a time");

      // Create the default server settings if none exist.
      if (settings == null)
        settings = new ServerSettings();

      // Create the NetManager.
      netMan = new NetManager(this) {
        AutoRecycle = true,
        IPv6Enabled = settings.IPv6,
      };

      cachedWriter = new NetDataWriter();

      // Start listening for packets.
      netMan.Start(settings.ListenPort);

      Debug.Log($"Server Listening on port {netMan.LocalPort}");

      IsServer = true;
      Server = this;
    }

    internal override void Destroy() {
      IsServer = false;
      Server = null;
    }
    public void OnConnectionRequest(ConnectionRequest request) {
      Debug.Log($"Connection request from {request.RemoteEndPoint.Address.ToString()}", this);
      request.Accept();
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError) {
      Debug.LogError($"Network Error: {socketError}", this);
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency) {
      // TODO: Show latency in a meaningful way.
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod) {
      // Read the packets using the packet processor.
      packetProcessor.ReadAllPackets(reader);
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) {
      Debug.Log($"Unconnected {messageType} message from {remoteEndPoint.Address.ToString()}", this);
    }

    public void OnPeerConnected(NetPeer peer) {
      NetChat.SendMessage($"Player joined the game");
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
      NetChat.SendMessage($"Player lost connection. Reason: {disconnectInfo.Reason}");
      throw new System.NotImplementedException();
    }
  }
}
