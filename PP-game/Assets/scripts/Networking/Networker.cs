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

    public bool Running {
      get {
        return Net != null && Net.IsRunning;
      }
    }

    public NetManager Net;
    internal NetDataWriter cachedWriter;
    public NetPacketProcessor PacketProcessor;
    [HideInInspector] public ChatController Chat;
    [HideInInspector] public EntityController Entities;

    public virtual void Init() {
      DontDestroyOnLoad(gameObject);

      cachedWriter = new NetDataWriter();
      PacketProcessor = new NetPacketProcessor();
      PacketProcessor.RegisterNestedType<Vector3>(Vector3Writer.Serialise, Vector3Writer.Deserialise);
      PacketProcessor.RegisterNestedType<Quaternion>(QuaternionWriter.Serialise, QuaternionWriter.Deserialise);

      Chat = GetComponentInChildren<ChatController>(false);
      Chat.Init(this);

      Entities = GetComponentInChildren<EntityController>(false);
      Entities.Init(this);
    }

    internal virtual void Tick(float deltaTime) { }

    private void Update() {
      if (Net != null && Net.IsRunning) {
        Net.PollEvents();
        Tick(Time.deltaTime);
      }
    }

    internal abstract void Destroy();
    private void OnDestroy() {
      Destroy();

      if(Chat != null)
        Chat.Shutdown();

      if(Entities != null)
        Entities.Shutdown();
    }

    internal virtual NetDataWriter WriteSerialisable<T>(T packet) where T : struct, INetSerializable {
      cachedWriter.Reset();
      packet.Serialize(cachedWriter);
      return cachedWriter;
    }
    internal virtual NetDataWriter WritePacket<T>(T packet) where T : class, new() {
      cachedWriter.Reset();
      PacketProcessor.Write(cachedWriter, packet);
      return cachedWriter;
    }

    internal void SendToAll(NetDataWriter writer, DeliveryMethod options) => Net.SendToAll(writer, options);

  }
}