using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PP.Networking;
using PP.Entities;

namespace PP.Entities.Player {
  public class PlayerEntity : NetworkedEntity {
    private void Awake() {

    }

    public override void Spawn(LiteNetLib.NetPacketReader extraSpawnData) {
      
    }

    public override void Send(LiteNetLib.Utils.NetDataWriter writer) {

    }
    public override void Receive(LiteNetLib.Utils.NetDataReader reader) {

    }
  }
}