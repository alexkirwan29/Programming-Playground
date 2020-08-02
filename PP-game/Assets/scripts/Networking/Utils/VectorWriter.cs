using LiteNetLib.Utils;
using UnityEngine;


namespace PP.Networking.Utils {
  public static class Vector3Writer {
    public static void Squash(ref Vector3 field, ref NetDataWriter writer) {
      writer.Put(field.x);
      writer.Put(field.y);
      writer.Put(field.z);
    }

    public static void UnSquash(ref Vector3 field, ref NetDataReader reader) {
      field.x = reader.GetFloat();
      field.y = reader.GetFloat();
      field.z = reader.GetFloat();
    }

    public static void Serialise(NetDataWriter writer, Vector3 vector) {
      writer.Put(vector.x);
      writer.Put(vector.y);
      writer.Put(vector.z);
    }
    public static Vector3 Deserialise(NetDataReader reader) {
      return new Vector3(reader.GetFloat(), reader.GetFloat(), reader.GetFloat());
    }
  }
}
