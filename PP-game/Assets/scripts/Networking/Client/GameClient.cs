using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using LiteNetLib;
using LiteNetLib.Utils;
using PP.Networking;
using System.Net;
using System.Net.Sockets;
using System;

namespace PP.Networking.Client {
  public class GameClient : Networker {

    public override void Init() {
      if (Client != null)
        throw new System.Exception("Only one client can be running");

      base.Init();

      Net.Start();

      IsClient = true;
      Client = this;
      listener.PeerDisconnectedEvent += OnDisconnected;
      listener.PeerConnectedEvent += OnConnected;
    }

    public UnityEvent<DisconnectReason, string> Disconnected;
    public UnityEvent Connected;

    internal override void Destroy() {
      if (Net != null && Net.IsRunning)
        Net.Stop(true);

      IsClient = false;
      Client = null;
    }

    public void Connect(string connectionString, JoinRequest request) {
      // Complain if the string is empty.
      if (string.IsNullOrWhiteSpace(connectionString))
        throw new System.NullReferenceException("Connection string is null or empty.");

      // Split the string on every colon.
      var parts = connectionString.Split(':');

      // Throw an error if the amount of strings returned are bigger than a host and port.
      if (parts.Length > 2)
        throw new ParseException("Malformed connection string, too many colons ( : )");

      // Set the host.
      string host = parts[0];

      // Looks like we might have a port.
      if (parts.Length == 2) {
        // Try and parse the port.
        if (ushort.TryParse(parts[1], out ushort parsedPort)) {
          // Cool let's attempt to connect to it.
          Connect(host, parsedPort, request);
        }

        // Something went wrong there.
        else
          throw new ParseException("Invalid port after colon");
      }

      // TODO: Look up DNS SRV records of the domain name and use that as the port.

      // Ahhh let's try it anyways with the default port. See what happens.
      Connect(host, 27015, request);

    }

    public void Connect(string host, ushort port, JoinRequest request) {
      Debug.Log($"Attempting connection to {host} on port {port}.");

      // Create our request packet.
      var writer = GetWriter();
      request.Serialize(writer);

      // Request to join.
      Net.Connect(host, port, writer);
    }

    private void OnDisconnected(NetPeer peer, DisconnectInfo disCon) {

      if (disCon.AdditionalData.TryGetString(out string message)) {
        Debug.Log($"Disconnected from server Reason: {disCon.Reason}, Message: {message}");
        if(Disconnected != null)
          Disconnected.Invoke(disCon.Reason, message);
      }
      else {
        Debug.Log($"Disconnected from server Reason: {disCon.Reason}");
        if(Disconnected != null)
          Disconnected.Invoke(disCon.Reason, null);
      }
    }
    private void OnConnected(NetPeer peer) {
      Debug.Log($"Connected to server at {peer.EndPoint}");
      if(Connected != null)
        Connected.Invoke();
    }
  }
}
