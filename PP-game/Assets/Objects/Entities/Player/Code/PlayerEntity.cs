using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PP.Networking;
using PP.Entities;
using PP.Networking.Utils;

using LiteNetLib.Utils;

namespace PP.Entities.Player {
  public class PlayerEntity : NetworkedEntity {

    public enum PlayerMessages : byte {
      UserDetails,
      SetIsMe,
      MoveFrame
    }
    private struct MoveFrame {
      int frame;
      Vector3 pos;
      float horAngle;
      float vertAngle;
    }

    Dictionary<int, MoveFrame> frames = new Dictionary<int, MoveFrame>();

    public string SkinUrl;
    public bool IsMe;

    private void OnMeChange() {
      if (IsMe) {
        if (TryGetComponent<Locomotion>(out var locomotion)) {
          locomotion.enabled = true;
        }
        if (TryGetComponent<Look>(out var look)) {
          look.enabled = true;
        }
      }
    }

    public override void NetTick(float deltaTime, int frame) {
      if(IsMe) {
        
      }
    }
    public override void Send(LiteNetLib.Utils.NetDataWriter writer) {

    }
    public override void Receive(LiteNetLib.Utils.NetDataReader reader) {
      var messageType = (PlayerMessages)reader.GetByte();

      if (messageType == PlayerMessages.UserDetails) {
        Messages.ReadDetails(reader, this);
      }
      else if (messageType == PlayerMessages.SetIsMe) {
        IsMe = reader.GetBool();
        OnMeChange();
      }
      else if (messageType == PlayerMessages.MoveFrame) {
        
      }
      else {
        Debug.LogError("Player Received an unknown message");
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