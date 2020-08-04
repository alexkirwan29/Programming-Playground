using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LiteNetLib;
using LiteNetLib.Utils;

namespace PP.Networking {
  public class NetworkedEntity : MonoBehaviour {
    public ushort Id;
    public int OwnerId;

    internal virtual void Spawnned() {

    }
    public virtual void NetTick(float deltaTime) {

    }

    public virtual void Send(NetDataWriter writer) {

    }
    public virtual void Receive(NetDataReader reader) {

    }
  }
}