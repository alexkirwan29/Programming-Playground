using LiteNetLib.Utils;
using UnityEngine;


namespace PP.Networking.Packets
{
  public static class Helpers
  {
    public static class Vector3Squash
    {
      public static void Squash(ref Vector3 field, ref NetDataWriter writer)
      {
        writer.Put(field.x);
        writer.Put(field.y);
        writer.Put(field.z);
      }

      public static void UnSquash(ref Vector3 field, ref NetDataReader reader)
      {
        field.x = reader.GetFloat();
        field.y = reader.GetFloat();
        field.z = reader.GetFloat();
      }
    }
  }
}