using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PP.Networking;
using PP.Entities;

using LiteNetLib.Utils;

namespace PP.Entities.Player {
  public class PlayerEntity : NetworkedEntity {

    public enum PlayerMessages : byte {
      UserDetails,
    }
    public string SkinURL;

    private void Awake() {

    }

    public override void Spawn() {

    }

    public override void Send(LiteNetLib.Utils.NetDataWriter writer) {

    }
    public override void Receive(LiteNetLib.Utils.NetDataReader reader) {
      var messageType = (PlayerMessages)reader.GetByte();

      if (messageType == PlayerMessages.UserDetails) {
        Messages.ReadDetails(reader, this);
      }
    }

    public static class Messages {
      public static void WriteDetails(NetDataWriter writer, string username, string skinUrl) {
        writer.Put((byte)PlayerMessages.UserDetails);

        writer.Put(username);
        writer.Put(skinUrl);
      }
      public static void ReadDetails(NetDataReader reader, PlayerEntity entity) {
        entity.name = reader.GetString();
        entity.SkinURL = reader.GetString();
      }
    }
  }
}