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
    public const int TPS = 30;

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

    private float tickStep = 1f / TPS;
    private float lastTick = 0;

    public virtual void Init() {
      DontDestroyOnLoad(gameObject);

      cachedWriter = new NetDataWriter();

      // Init all NetControllers in children.
      var components = GetComponentsInChildren<NetController>(false);
      controllers = new Dictionary<byte, NetController>();
      foreach (var component in components) {
        controllers.Add(component.GetId(), component);
        component.Init(this);
      }

      // Get the chat and entities controller and save them, we'll be using them quite a bit.
      Chat = GetController<ChatController>(ChatController.CONTROLLER_ID);
      Entities = GetController<EntityController>(EntityController.CONTROLLER_ID);

      // Setup networking events and controllers.
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

    /// <summary>
    /// Gets an inited controller used by this networker.
    /// </summary>
    /// <param name="id">The id of the controller.</param>
    /// <returns>A controller ready to go.</returns>
    public T GetController<T>(byte id) where T : NetController {
      if (controllers.TryGetValue(id, out NetController c))
        return (T)c;
      else
        return null;
    }

    internal virtual void Tick(float deltaTime) {
      Entities.NetTick(deltaTime);
    }

    private void Update() {
      if (Net != null && Net.IsRunning) {
        Net.PollEvents();

        float time = Time.timeSinceLevelLoad;

        if(lastTick + tickStep > time) {
          Tick(time - lastTick);
          lastTick = time;
        }
      }
    }

    private void OnDestroy() {
      Shutdown();

      if(Net != null)
        Net.Stop();
    }
    internal virtual void Shutdown() {
      // Shutdown all controllers and clear the list.
      if (controllers != null) {
        foreach (var controller in controllers.Values)
          controller.Shutdown();
        controllers.Clear();
      }

      if (Net != null)
        Net.DisconnectAll();
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

    public void SendToAll<T>(T packet, DeliveryMethod options) where T : INetSerializable {
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