﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LiteNetLib;
using LiteNetLib.Utils;
using PP.Networking;
using System.Net;
using System.Net.Sockets;

namespace PP.Networking.Server
{
  public class GameServer : MonoBehaviour, INetEventListener
  {
    public void OnConnectionRequest(ConnectionRequest request)
    {
      throw new System.NotImplementedException();
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
      throw new System.NotImplementedException();
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
      throw new System.NotImplementedException();
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {
      throw new System.NotImplementedException();
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
      throw new System.NotImplementedException();
    }

    public void OnPeerConnected(NetPeer peer)
    {
      throw new System.NotImplementedException();
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
      throw new System.NotImplementedException();
    }
  }
}
