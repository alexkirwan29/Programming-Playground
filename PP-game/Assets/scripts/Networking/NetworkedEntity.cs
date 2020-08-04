using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LiteNetLib;
using LiteNetLib.Utils;

namespace PP.Networking {
  public class NetworkedEntity : MonoBehaviour {
    public ushort Id;
    public ushort PrefabId;
    public Networker Net;

    internal virtual void Spawnned(Networker net) {
      Net = net;
    }
    public virtual void NetTick(float deltaTime, int netFrame) {

    }

    public virtual void Send(NetDataWriter writer) {

    }
    public virtual void Receive(NetDataReader reader) {

    }
  }
}