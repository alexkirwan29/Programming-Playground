using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LiteNetLib;
using LiteNetLib.Utils;
using PP.Networking;
using PP.Entities.Player;
using System.Net;
using System.Net.Sockets;

namespace PP.Networking.Server {
  public class GameServer : Networker {

    private ServerSettings settings;
    public Dictionary<ushort, PlayerEntity> players;
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

      players = new Dictionary<ushort, PlayerEntity>();
      Net.Start(settings.ListenPort);

      IsServer = true;
      Server = this;

      listener.ConnectionRequestEvent += OnConnectionRequest;
      listener.PeerConnectedEvent += OnPeerConnected;
      listener.PeerDisconnectedEvent += OnPeerDisconnected;
      listener.NetworkReceiveUnconnectedEvent += OnNetworkReceiveUnconnected;

      Debug.Log($"Listening on port {Net.LocalPort}", this);
    }

    internal override void Destroy() {
      IsServer = false;
      Server = null;
    }

    private void OnConnectionRequest(ConnectionRequest request) {
      Debug.Log($"Connection request from {request.RemoteEndPoint.Address}", this);
      request.Accept();
    }

    private void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) {
      Debug.Log($"Unconnected {messageType} message from {remoteEndPoint.Address}", this);
    }

    private void OnPeerConnected(NetPeer peer) {
      Chat.BroadcastChatMessage($"Player joined the game");
      Entities.SpawnEntity(100, 0, Vector3.zero, Quaternion.identity);
    }

    private void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
      Chat.BroadcastChatMessage($"Player lost connection. Reason: {disconnectInfo.Reason}");
    }
  }
}
