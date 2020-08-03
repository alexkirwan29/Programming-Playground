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

    public const string GAME_VERSION = "0.0.1_rip";

    public bool Running {
      get {
        return Net != null && Net.IsRunning;
      }
    }

    public NetManager Net;
    internal EventBasedNetListener listener;
    internal NetDataWriter cachedWriter;

    internal NetSerializer serializer;

    public Dictionary<byte, NetController> controllers;

    [HideInInspector] public ChatController Chat;
    [HideInInspector] public EntityController Entities;

    public virtual void Init() {
      DontDestroyOnLoad(gameObject);

      cachedWriter = new NetDataWriter();

      var components = GetComponentsInChildren<NetController>(false);
      controllers = new Dictionary<byte, NetController>();
      foreach (var component in components) {
        controllers.Add(component.GetId(), component);
      }

      Chat = GetController<ChatController>(ChatController.CONTROLLER_ID);
      Chat.Init(this);

      Entities = GetController<EntityController>(EntityController.CONTROLLER_ID);
      Entities.Init(this);

      listener = new EventBasedNetListener();

      listener.NetworkReceiveEvent += OnNetworkReceive;
      listener.NetworkErrorEvent += OnNetworkError;

      serializer = new NetSerializer();

      Net = new NetManager(listener) {
        AutoRecycle = true
      };

    }

    internal void OnNetworkError(IPEndPoint endPoint, SocketError socketError) {
      Debug.LogError($"Network Error: {socketError}", this);
    }

    internal virtual void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod method) {
      if (reader.TryGetByte(out byte controllerId)) {
        if (controllers.TryGetValue(controllerId, out NetController controller)) {
          controller.Read(reader, peer);
        }
        else
          Debug.LogError("A packet for an unknown controller id was received.", this);
      }
      else
        Debug.LogError("A packet was received without a controller id.", this);
    }

    public T GetController<T>(byte id) where T : NetController {
      if (controllers.TryGetValue(id, out NetController c))
        return (T)c;
      else
        return null;
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

      if (Chat != null)
        Chat.Shutdown();

      if (Entities != null)
        Entities.Shutdown();
    }


    internal virtual NetDataWriter WriteSerialisable<T>(T packet) where T : struct, INetSerializable {
      cachedWriter.Reset();
      packet.Serialize(cachedWriter);
      return cachedWriter;
    }
    internal virtual NetDataWriter Write<T>(T packet) where T : class, new() {
      cachedWriter.Reset();
      serializer.Serialize(cachedWriter, packet);
      return cachedWriter;
    }

    public void SendToAll(NetDataWriter writer, DeliveryMethod options) => Net.SendToAll(writer, options);

    public void SendToAll<T>(T packet, DeliveryMethod options) where T: INetSerializable
    {
      cachedWriter.Reset();
      packet.Serialize(cachedWriter);
      Net.SendToAll(cachedWriter, options);
    }

    [System.Obsolete]
    public NetDataWriter GetWriter() {
      cachedWriter.Reset();
      return cachedWriter;
    }
    public NetDataWriter GetWriter(NetController controller) {
      cachedWriter.Reset();
      cachedWriter.Put(controller.GetId());
      return cachedWriter;
    }
  }
}