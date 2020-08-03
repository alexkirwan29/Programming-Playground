using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LiteNetLib;
using LiteNetLib.Utils;
using System;

namespace PP.Networking {
  public class EntityController : MonoBehaviour {
    Dictionary<ushort, NetworkedEntity> entities;
    Dictionary<ushort, NetworkedEntity> prefabs;
    ushort idCounter = 0;

    Networker networker;

    public void Setup(Networker networker, Dictionary<ushort, NetworkedEntity> prefabs) {
      this.networker = networker;
      entities = new Dictionary<ushort, NetworkedEntity>();
      this.prefabs = prefabs;
      networker.packetProcessor.SubscribeReusable<Packets.Spawn>(OnSpawn);
      networker.packetProcessor.SubscribeReusable<Packets.Destroy>(OnDestroyEntity);
    }

    public void OnSpawn(Packets.Spawn packet) {

      if (entities.ContainsKey(packet.id)) {
        Debug.LogWarning($"Entity {packet.id} with prefab already exists, destroying and replacing.");
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

    public void OnDestroyEntity(Packets.Destroy packet) {
      if (!Networker.IsClient)
        throw new NotClientException();

      DestroyEntity(packet.id, packet.silent);
    }

    public void SpawnEntity(ushort prefab, Vector3 position) {
      if (!Networker.IsServer)
        throw new NotServerException();
      // Create the spawn packet.

      var packet = new Packets.Spawn(prefab, idCounter++, 0, position);

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
  }
}