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
    
    private ServerSettings settings;
    public void Init(ServerSettings settings = null) {
      // Don't run if a server already exists.
      if (Server != null) {
        enabled = false;
        throw new System.Exception("Only one server can at a time");
      }
      
      base.Init();

      // Create or use provided server settings.
      if(settings == null)
        settings = new ServerSettings();
      this.settings = settings;

      // Start the net manager.
      Net = new NetManager(this) {
        AutoRecycle = true,
        IPv6Enabled = settings.IPv6
      };
      Net.Start(settings.ListenPort);

      IsServer = true;
      Server = this;

      Debug.Log($"Listening on port {Net.LocalPort}", this);
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
      PacketProcessor.ReadAllPackets(reader);
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) {
      Debug.Log($"Unconnected {messageType} message from {remoteEndPoint.Address.ToString()}", this);
    }

    public void OnPeerConnected(NetPeer peer) {
      Chat.BroadcastChatMessage($"Player joined the game");
      GetComponent<EntityController>().SpawnEntity(100, Vector3.zero);
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
      Chat.BroadcastChatMessage($"Player lost connection. Reason: {disconnectInfo.Reason}");
    }

    public void SendToAll<T>(T packet, DeliveryMethod options) where T: INetSerializable
    {
      cachedWriter.Reset();
      packet.Serialize(cachedWriter);
      Net.SendToAll(cachedWriter, options);
    }
  }
}
