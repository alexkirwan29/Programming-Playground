using System.Collections.Generic;
using UnityEngine;

using LiteNetLib;
using LiteNetLib.Utils;

using PP.Networking.Utils;

namespace PP.Networking {
  public class EntityController : MonoBehaviour, INetController {
    Dictionary<ushort, NetworkedEntity> entities;

    [SerializeField]
    private PrefabList prefabList;
    Dictionary<ushort, NetworkedEntity> prefabs;
    ushort idCounter = 0;

    Networker networker;

    public void Init(Networker networker) {
      this.networker = networker;

      entities = new Dictionary<ushort, NetworkedEntity>();
      prefabs = prefabList.GetPrefabs();

      networker.PacketProcessor.SubscribeReusable<Spawn>(OnSpawn);
      networker.PacketProcessor.SubscribeReusable<Destroy>(OnDestroyEntity);

      Debug.Log("Started", this);
    }

    public void Shutdown() {
      networker.PacketProcessor.RemoveSubscription<Spawn>();
      networker.PacketProcessor.RemoveSubscription<Destroy>();
      Debug.Log("Shutdown", this);
    }

    public bool Ready => networker != null && networker.Running;

    public void OnSpawn(Spawn packet) {

      if (entities.ContainsKey(packet.id)) {
        Debug.LogWarning($"Entity {packet.id} already exists, destroying and replacing.");
        DestroyEntity(packet.id, true);
      }

      // Instantiate the prefab.
      var go = GameObject.Instantiate(prefabs[packet.prefab].gameObject);

      // Get the Entity component and set some values.
      var entity = go.GetComponent<NetworkedEntity>();
      entity.Id = packet.id;
      entity.Spawn();

      // Add the new entity to the list.
      entities.Add(packet.id, entity);
    }

    public void OnDestroyEntity(Destroy packet) {
      if (!Networker.IsClient)
        throw new NotClientException();

      DestroyEntity(packet.id, packet.silent);
    }

    public void SpawnEntity(ushort prefab, Vector3 position) {
      if (!Networker.IsServer)
        throw new NotServerException();
      // Create the spawn packet.

      var packet = new Spawn(prefab, idCounter++, 0, position);

      // Spawn the prefab on the server.
      OnSpawn(packet);

      Networker.Server.SendToAll(packet, DeliveryMethod.ReliableUnordered);
    }

    public void SendStateToClient() {
      if (!Networker.IsServer)
        throw new NotServerException();
    }

    private void DestroyEntity(ushort id, bool silent = false) {
      if (!entities.ContainsKey(id))
        Debug.LogWarning($"Entity [{id}] does not exist.");

      entities[id].DestroyEntity(silent);
      entities.Remove(id);
    }

    public class Spawn : INetSerializable {
      public ushort id;
      public ushort prefab;
      public ushort owner;
      public Vector3 position;
      public bool simple;
      public Quaternion rotation = Quaternion.identity;
      public Vector3 scale = Vector3.one;

      public Spawn() {

      }

      public Spawn(ushort prefab, ushort id, ushort owner, Vector3 position) {
        this.prefab = prefab;
        this.id = id;
        this.owner = owner;
        this.position = position;
      }

      public void Deserialize(NetDataReader reader) {

        // Read the useful stuff.
        prefab = reader.GetUShort();
        id = reader.GetUShort();
        owner = reader.GetUShort();
        position = Vector3Writer.Deserialise(reader);

        // Read the extra cool stuff.
        if (reader.GetBool()) {
          rotation = QuaternionWriter.Deserialise(reader);
          scale = Vector3Writer.Deserialise(reader);
        }
      }
      public void Serialize(NetDataWriter writer) {

        // Write the useful stuff.
        writer.Put(prefab);
        writer.Put(id);
        writer.Put(owner);
        Vector3Writer.Serialise(writer, position);

        // Write the extra cool stuff.
        if (rotation != Quaternion.identity || scale != Vector3.one) {
          QuaternionWriter.Serialise(writer, rotation);
          Vector3Writer.Serialise(writer, scale);
        }
      }
    }
    public class Destroy {
      public ushort id { get; set; }
      public bool silent { get; set; }

      public Destroy() { }
    }
  }
}