using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LiteNetLib;
using LiteNetLib.Utils;
using PP.Networking;
using PP.Networking.Utils;
using System.Net;
using System.Net.Sockets;

namespace PP.Networking {
  public abstract class Networker : MonoBehaviour {
    public static bool IsServer;
    public static bool IsClient;
    public static Server.GameServer Server;
    public static Client.GameClient Client;

    public bool IsRunning {
      get {
        return netMan.IsRunning;
      }
    }

    public NetManager netMan;
    internal NetDataWriter cachedWriter;

    private bool HasInit = false;

    public NetPacketProcessor packetProcessor;

    public abstract void Create();
    internal abstract void Destroy();

    internal virtual void DoUpdate(float dTime) {

    }
  
    public virtual void Init() {
      DontDestroyOnLoad(gameObject);

      cachedWriter = new NetDataWriter();
      packetProcessor = new NetPacketProcessor();
      packetProcessor.RegisterNestedType<Vector3>(Vector3Writer.Serialise, Vector3Writer.Deserialise);
      packetProcessor.RegisterNestedType<Quaternion>(QuaternionWriter.Serialise, QuaternionWriter.Deserialise);

      HasInit = true;
      NetChat.Initialise(this);
    }

    private void Update() {
      if (HasInit)
        DoUpdate(Time.deltaTime);

      if (netMan != null && netMan.IsRunning)
        netMan.PollEvents();
    }

    private void OnDestroy() {
      if (HasInit) {
        NetChat.DeInitialise();
      }

      Destroy();
    }

    internal virtual NetDataWriter WriteSerialisable<T>(T packet) where T : struct, INetSerializable {
      cachedWriter.Reset();
      packet.Serialize(cachedWriter);
      return cachedWriter;
    }
    internal virtual NetDataWriter WritePacket<T>(T packet) where T : class, new() {
      cachedWriter.Reset();
      packetProcessor.Write(cachedWriter, packet);
      return cachedWriter;
    }
  }
}