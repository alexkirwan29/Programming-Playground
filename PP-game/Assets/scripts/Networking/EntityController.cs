using System.Collections.Generic;
using UnityEngine;

using LiteNetLib;
using LiteNetLib.Utils;

using PP.Networking.Utils;

namespace PP.Networking {
  public class EntityController : NetController {
    public const byte CONTROLLER_ID = 10;

    /// <summary>
    /// The controller identifier.
    /// </summary>
    /// <returns>Returns an identifier for this controller.</returns>
    public override byte GetId() => CONTROLLER_ID;

    [SerializeField]
    private PrefabList prefabList;

    Dictionary<ushort, NetworkedEntity> entities;
    Dictionary<ushort, NetworkedEntity> prefabs;

    ushort idCounter = 0;



    /// <summary>
    /// Initialises the controller.
    /// </summary>
    /// <param name="networker">The networker that this controller will be using.</param>
    public override void Init(Networker networker) {
      base.Init(networker);

      entities = new Dictionary<ushort, NetworkedEntity>();
      prefabs = prefabList.GetPrefabs();

      Debug.Log("Started", this);
    }



    public override void Shutdown() {
      Debug.Log("Shutdown", this);

      // Delete ALL spawnned entities.
      foreach (var id in entities.Keys)
        DestroyEntity(id);

      entities.Clear();
    }



    public override void Read(NetPacketReader reader, NetPeer peer) {
      if (Networker.IsClient) {
        // Get the command from this packet.
        var command = (EntityCommand)reader.GetByte();

        if (command == EntityCommand.Destroy) {
          DestroyEntity(reader.GetUShort());
        }
        else if (command == EntityCommand.Spawn) {
          SpawnEntity(reader);
        }
        else if (command == EntityCommand.Extra) {
          entities[reader.GetUShort()].Receive(reader);
        }
        else {
          Debug.LogError("Received a packet with an unexpected command.", this);
        }
      }
      base.Read(reader, peer);
    }



    /// <summary>
    /// Spawns an entity at a given location if we are the server.
    /// </summary>
    /// <param name="prefabId">The ID of the prefab.</param>
    /// <param name="owner">The owner of this new entity.</param>
    /// <param name="pos">The position to spawn this new entity at.</param>
    /// <param name="rot">The rotation to spawn this new entity at.</param>
    public T SpawnEntity<T>(ushort prefabId, int owner, Vector3 pos, Quaternion rot) where T : NetworkedEntity {
      if (!Networker.IsServer)
        throw new NotServerException();

      // Spawn the prefab on the server.
      var entity = Instantiate<T>(idCounter++, prefabId, owner, pos, rot);
      entities.Add(entity.Id, entity);

      // Construct our packet.
      var writer = net.GetWriter(this);
      writer.Put((byte)EntityCommand.Spawn);
      writer.Put(entity.Id);
      writer.Put(owner);
      writer.Put(prefabId);
      Vector3Writer.Serialise(writer, pos);
      QuaternionWriter.Serialise(writer, rot);

      // Send the packet to everyone.
      net.SendToAll(writer, DeliveryMethod.ReliableOrdered);

      return entity;
    }


    /// <summary>
    /// Destroys an entity. If we are the server, broadcast this too.
    /// </summary>
    /// <param name="id">The Id of the entity we want to destroy.</param>
    /// <param name="silent">Should the entity "become dissapear" or go out with a bang!</param>
    public void DestroyEntity(ushort id) {
      if (!entities.ContainsKey(id))
        Debug.LogWarning($"Entity [{id}] does not exist.", this);

      // Destroy the entity on the server or client.
      Destroy(entities[id].gameObject);
      entities.Remove(id);

      // Only send these changes if we are the server.
      if (Networker.IsServer) {
        // Create the packet.
        var writer = net.GetWriter(this);
        writer.Put((byte)EntityCommand.Destroy);
        writer.Put(id);

        // Send the packet to everyone.
        net.SendToAll(writer, DeliveryMethod.ReliableUnordered);
      }
    }



    /// <summary>
    /// Prepare a message to be sent to a particular entity.
    /// </summary>
    /// <param name="writer">The NetDataWriter</param>
    /// <param name="entity"The entity to prepare the message for></param>
    public void PrepareMessage(NetDataWriter writer, NetworkedEntity entity) => PrepareMessage(writer, entity.Id);

    /// <summary>
    /// Prepare a message to be sent to a particular entity.
    /// </summary>
    /// <param name="writer">The NetDataWriter</param>
    /// <param name="entity"The entity to prepare the message for></param>
    public void PrepareMessage(NetDataWriter writer, ushort entity) {
      writer.Put((byte)EntityCommand.Extra);
      writer.Put(entity);
    }




    private void SpawnEntity(NetPacketReader reader) {
      // If we are the server leave, we have already done this.
      if (Networker.IsServer)
        return;

      // Get the entity ID.
      ushort id = reader.GetUShort();

      // If an entity with this ID already exists, destroy it.
      if (entities.ContainsKey(id)) {
        Debug.LogWarning($"Entity {id} already exists, destroying and replacing.", this);
        DestroyEntity(id);
      }

      // Get the owner, prefabId, position and rotation of this prefab.
      int owner = reader.GetInt();
      ushort prefabId = reader.GetUShort();
      Vector3 pos = Vector3Writer.Deserialise(reader);
      Quaternion rot = QuaternionWriter.Deserialise(reader);

      // Create an instance of this prefab, put it in the list of entities.
      var entity = Instantiate<NetworkedEntity>(id, prefabId, owner, pos, rot);
      entities.Add(id, entity);
    }


    private T Instantiate<T>(ushort id, ushort prefabId, int owner, Vector3 pos, Quaternion rot) where T : NetworkedEntity {
      Debug.Log("Instantiate " + prefabs.Count);
      if (prefabs.TryGetValue(prefabId, out NetworkedEntity prefab)) {
        // Instantiate the prefab.
        var go = (GameObject)Instantiate(prefab.gameObject, pos, rot);

        // Setup some stuff on the entity.
        var entity = go.GetComponent<T>();
        entity.Id = id;
        entity.OwnerId = owner;
        entity.Spawnned();
        return entity;
      }
      return null;
    }

    internal void NetTick(float deltaTime) {
      foreach (var entity in entities.Values) {
        entity.NetTick(deltaTime);
      }
    }



    enum EntityCommand : byte {
      Spawn,
      Destroy,
      PositionSync,
      Extra,
    }
  }
}