using UnityEngine;
using LiteNetLib.Utils;
using PP.Networking.Utils;

namespace PP.Networking.Packets {

  public class Spawn : INetSerializable {
    public ushort id;
    public ushort prefab;
    public ushort owner;
    public Vector3 position;
    public bool simple;
    public Quaternion rotation = Quaternion.identity;
    public Vector3 scale = Vector3.one;

    public Spawn()
    {

    }

    public Spawn(ushort prefab, ushort id, ushort owner, Vector3 position)
    {
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

    public Destroy() {}
  }
}