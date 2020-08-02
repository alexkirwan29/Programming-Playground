using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LiteNetLib;
using LiteNetLib.Utils;

namespace PP.Networking
{
  public class EntityController
  {
    Dictionary<ushort, NetworkedEntity> entities;
    Dictionary<ushort, NetworkedEntity> prefabs;

    Networker networker;

    public EntityController(Networker networker)
    {
      this.networker = networker;
      entities = new Dictionary<ushort, NetworkedEntity>();

      if(Networker.IsClient)
      {
        networker.packetProcessor.SubscribeNetSerializable<Packets.Spawn>(OnSpawn);
        networker.packetProcessor.SubscribeReusable<Packets.Destroy>(OnDestroy);
      }
    }

    public void OnSpawn(Packets.Spawn packet)
    {
      if(!Networker.IsClient)
        throw new NotClientException();

      if(entities.ContainsKey(packet.id))
      {
        Debug.LogWarning($"Entity {packet.id} with prefab already exists, destroying and replacing.");
        DestroyEntity(packet.id, true);
      }

      // Instantiate the prefab.
      var go = GameObject.Instantiate(prefabs[packet.id].gameObject, packet.position, packet.rotation);
      go.transform.localScale = packet.scale;

      // Get the Entity component and set some values.
      var entity = go.GetComponent<NetworkedEntity>();
      entity.Id = packet.id;
      entity.OwnerId = packet.owner;
      entity.Spawn();

      // Add the new entity to the list.
      entities.Add(packet.id, entity);
    }
    public void OnDestroy(Packets.Destroy packet)
    {
      if(!Networker.IsClient)
        throw new NotClientException();
      
      DestroyEntity(packet.id, packet.silent);
    }
    public void SendStateToClient()
    {
      if(!Networker.IsServer)
        throw new NotServerException();
    }

    private void DestroyEntity(ushort id, bool silent = false)
    {
      if(!entities.ContainsKey(id))
        Debug.LogWarning($"Entity [{id}] does not exist.");

      entities[id].DestroyEntity(silent);
      entities.Remove(id);
    }
  }
}