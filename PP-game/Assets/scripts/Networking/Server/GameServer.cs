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
    public Dictionary<int, PlayerEntity> players;
    private Dictionary<IPEndPoint, JoinRequest> newPlayers;
    public void Init(ServerSettings settings = null) {
      // Don't run if a server already exists.
      if (Server != null) {
        enabled = false;
        throw new System.Exception("Only one server can at a time");
      }

      base.Init();

      // Create or use provided server settings.
      if (settings == null)
        settings = new ServerSettings();
      this.settings = settings;

      players = new Dictionary<int, PlayerEntity>();
      newPlayers = new Dictionary<IPEndPoint, JoinRequest>();
      Net.Start(settings.ListenPort);

      IsServer = true;
      Server = this;

      listener.ConnectionRequestEvent += OnConnectionRequest;
      listener.PeerConnectedEvent += OnPeerConnected;
      listener.PeerDisconnectedEvent += OnPeerDisconnected;
      listener.NetworkReceiveUnconnectedEvent += OnNetworkReceiveUnconnected;

      Debug.Log($"Listening on port {Net.LocalPort}", this);
    }

    internal override void Shutdown() {
      base.Shutdown();
      IsServer = false;
      Server = null;
    }

    private void OnConnectionRequest(ConnectionRequest request) {

      // Allow the request if they passed the Check.
      if (JoinRequest.CheckRequest(request.Data, settings.Password, out string reason, out JoinRequest joinRequest)) {
        request.Accept();
        newPlayers.Add(request.RemoteEndPoint, joinRequest);
        Debug.Log($"Accepted connection from {joinRequest.Username} at {request.RemoteEndPoint}.", this);
      }

      // Reject the request and be nice enough to give a reason.
      else {
        cachedWriter.Reset();
        cachedWriter.Put(reason);
        request.Reject(cachedWriter);

        Debug.Log($"Rejected connection from {request.RemoteEndPoint}. Reason: {reason}", this);
      }
    }

    private void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) {
      Debug.Log($"Unconnected {messageType} message from {remoteEndPoint.Address}", this);
    }

    private void OnPeerConnected(NetPeer peer) {

      var data = newPlayers[peer.EndPoint];
      newPlayers.Remove(peer.EndPoint);

      // Spawn the entity and add it to the list of players.
      var newPlayer = Entities.SpawnEntity<PlayerEntity>(100, peer.Id, Vector3.zero, Quaternion.identity);
      players.Add(peer.Id, newPlayer);

      // Set the username and SkinUrl of this user.
      newPlayer.name = data.Username;
      newPlayer.SkinUrl = data.SkinUrl;

      // Send the username and SkinUrl of this user to all the other players.
      var writer = GetWriter(Entities);
      Entities.PrepareMessage(writer, newPlayer);
      PlayerEntity.Messages.WriteDetails(writer, newPlayer);
      SendToAll(writer, DeliveryMethod.ReliableOrdered);

      // Let the world know they've joined.
      Chat.BroadcastChatMessage($"{newPlayer.name} joined the game.");
    }

    private void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
      Entities.DestroyEntity(players[peer.Id].Id);
      players.Remove(peer.Id);
      Chat.BroadcastChatMessage($"{players[peer.Id].name} left the game. Reason: {disconnectInfo.Reason}");
    }
  }
}
