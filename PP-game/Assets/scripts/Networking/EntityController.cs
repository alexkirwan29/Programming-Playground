using System.Collections.Generic;
using UnityEngine;

using LiteNetLib;
using LiteNetLib.Utils;

using PP.Networking.Utils;
using PP.Entities.Player;

namespace PP.Networking {
  public class EntityController : NetController {
    public const byte CONTROLLER_ID = 10;
    public const byte CHANNEL = 110;

    /// <summary>
    /// The controller identifier.
    /// </summary>
    /// <returns>Returns an identifier for this controller.</returns>
    public override byte GetId() => CONTROLLER_ID;

    [SerializeField]
    private PrefabList prefabList;

    internal Dictionary<ushort, NetworkedEntity> Entities;
    Dictionary<ushort, NetworkedEntity> prefabs;

    ushort idCounter = 0;



    /// <summary>
    /// Initialises the controller.
    /// </summary>
    /// <param name="networker">The networker that this controller will be using.</param>
    public override void Init(Networker networker) {
      base.Init(networker);

      Entities = new Dictionary<ushort, NetworkedEntity>();
      prefabs = prefabList.GetPrefabs();

      Debug.Log("Started", this);
    }



    public override void Shutdown() {
      Debug.Log("Shutdown", this);

      // Delete ALL spawnned entities.
      ushort[] ids = new ushort[Entities.Count];
      Entities.Keys.CopyTo(ids, 0);
      for (int i = 0; i < ids.Length; i++) {
        DestroyEntity(ids[i]);
      }

      Entities.Clear();
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
          Entities[reader.GetUShort()].Receive(reader);
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
    /// <param name="pos">The position to spawn this new entity at.</param>
    /// <param name="rot">The rotation to spawn this new entity at.</param>
    public T SpawnEntity<T>(ushort prefabId, Vector3 pos, Quaternion rot) where T : NetworkedEntity {
      if (!Networker.IsServer)
        throw new NotServerException();

      // Spawn the prefab on the server.
      var entity = Instantiate<T>(idCounter++, prefabId, pos, rot);
      Entities.Add(entity.Id, entity);

      // Sync this entity's spawn.
      net.SendToAll(SpawnSync(entity), DeliveryMethod.ReliableOrdered);

      return entity;
    }


    /// <summary>
    /// Destroys an entity. If we are the server, broadcast this too.
    /// </summary>
    /// <param name="id">The Id of the entity we want to destroy.</param>
    /// <param name="silent">Should the entity "become dissapear" or go out with a bang!</param>
    public void DestroyEntity(ushort id) {
      if (!Entities.ContainsKey(id)) {
        Debug.LogWarning($"Entity [{id}] does not exist.", this);
        return;
      }

      // Destroy the entity on the server or client.
      Destroy(Entities[id].gameObject);
      Entities.Remove(id);

      // Only send these changes if we are the server.
      if (Networker.IsServer) {
        // Create the packet.
        var writer = PrepareMessage(id, EntityCommand.Destroy);

        // Send the packet to everyone.
        net.SendToAll(writer, DeliveryMethod.ReliableUnordered);
      }
    }

    public void SpawnSyncAllTo(NetPeer peer, PlayerEntity exclude) {
      // Send all the prefabs to the new player.
      foreach (var entity in Entities.Values) {
        if(entity != exclude)
          peer.Send(SpawnSync(entity), DeliveryMethod.ReliableOrdered);
      }
    }

    public NetDataWriter SpawnSync(NetworkedEntity entity) {
      // Construct our packet.
      var writer = PrepareMessage(entity, EntityCommand.Spawn);
      writer.Put(entity.PrefabId);
      Vector3Writer.Serialise(writer, entity.transform.position);
      QuaternionWriter.Serialise(writer, entity.transform.rotation);

      return writer;
    }


    /// <summary>
    /// Prepare a message to be sent to a particular entity.
    /// </summary>
    /// <param name="entity"The entity to prepare the message for></param>
    public NetDataWriter PrepareMessage(NetworkedEntity entity, EntityCommand command) => PrepareMessage(entity.Id, command);

    /// <summary>
    /// Prepare a message to be sent to a particular entity.
    /// </summary>
    /// <param name="entity"The entity to prepare the message for></param>
    public NetDataWriter PrepareMessage(ushort entity, EntityCommand command) {
      var writer = net.GetWriter(this);
      writer.Put((byte)command);
      writer.Put(entity);
      return writer;
    }




    private void SpawnEntity(NetPacketReader reader) {
      // If we are the server leave, we have already done this.
      if (Networker.IsServer)
        return;

      // Get the entity ID.
      ushort id = reader.GetUShort();

      // If an entity with this ID already exists, destroy it.
      if (Entities.ContainsKey(id)) {
        Debug.LogWarning($"Entity {id} already exists, destroying and replacing.", this);
        DestroyEntity(id);
      }

      // Get the prefabId, position and rotation of this prefab.
      ushort prefabId = reader.GetUShort();
      Vector3 pos = Vector3Writer.Deserialise(reader);
      Quaternion rot = QuaternionWriter.Deserialise(reader);

      // Create an instance of this prefab, put it in the list of entities.
      var entity = Instantiate<NetworkedEntity>(id, prefabId, pos, rot);
      Entities.Add(id, entity);
    }


    private T Instantiate<T>(ushort id, ushort prefabId, Vector3 pos, Quaternion rot) where T : NetworkedEntity {
      if (prefabs.TryGetValue(prefabId, out NetworkedEntity prefab)) {
        // Instantiate the prefab.
        var go = (GameObject)Instantiate(prefab.gameObject, pos, rot);

        // Setup some stuff on the entity.
        var entity = go.GetComponent<T>();
        entity.Id = id;
        entity.PrefabId = prefabId;
        entity.Spawnned(net);
        return entity;
      }
      return null;
    }

    internal void NetTick(float deltaTime, int tickTime) {
      foreach (var entity in Entities.Values) {
        entity.NetTick(deltaTime, tickTime);
      }
    }



    public enum EntityCommand : byte {
      Spawn,
      Destroy,
      PositionSync,
      Extra,
    }
  }
}