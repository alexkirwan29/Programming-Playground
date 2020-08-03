using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LiteNetLib;
using LiteNetLib.Utils;
using PP.Networking;
using System.Net;
using System.Net.Sockets;

namespace PP.Networking.Client {
  public class GameClient : Networker {

    public override void Init() {
      if (Client != null)
        throw new System.Exception("Only one client can be running");
      
      base.Init();

      Net.Start();

      IsClient = true;
      Client = this;
    }

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
      serializer.Serialize(writer, request);

      // Request to join.
      Net.Connect(host, port, writer);
    }
  }
}
