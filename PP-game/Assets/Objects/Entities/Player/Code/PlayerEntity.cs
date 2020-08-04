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
    public string SkinUrl;

    internal override void Spawnned() {
      if(PP.Networking.Client.GameClient.MyID == Id) {
        if(TryGetComponent<Locomotion>(out var locomotion)) {
          locomotion.enabled = true;
        }
        if(TryGetComponent<Look>(out var look)) {
          look.enabled = true;
        }
      }
    }
    public override void NetTick(float deltaTime) {
      
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
      public static void WriteDetails(NetDataWriter writer, PlayerEntity player) {
        writer.Put((byte)PlayerMessages.UserDetails);

        writer.Put(player.name);
        writer.Put(player.SkinUrl);
      }
      public static void ReadDetails(NetDataReader reader, PlayerEntity entity) {
        entity.name = reader.GetString();
        entity.SkinUrl = reader.GetString();
      }
    }
  }
}