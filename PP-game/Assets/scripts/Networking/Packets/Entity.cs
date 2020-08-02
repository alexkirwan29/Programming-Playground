using UnityEngine;
using LiteNetLib.Utils;

namespace PP.Networking.Packets
{
  public class Move
  {
    public int EntityID;
    public Vector3 position;
    public Vector3 lookNormal;
    public Vector3 velocity;
  }

  [System.Flags]
  public enum MovementInput : byte
  {
    Left = 1 << 1,
    Right = 1 << 2,
    Forward = 1 << 3,
    Backwards = 1 << 4,
    Jump = 1 << 5,
    Crouch = 1 << 6,
    Run = 1 << 7
  }

  public class PlayerInput : INetSerializable
  {
    public ushort Id;
    public MovementInput MoveInput;
    // TODO Shrink down the size of this variable to take less bytes.
    public Vector3 LookNormal;
    public ushort ServerTick;

    public void Deserialize(NetDataReader reader)
    {
      Id = reader.GetUShort();
      MoveInput = (MovementInput)reader.GetByte();
      LookNormal = Utils.Vector3Writer.Deserialise(reader);
      ServerTick = reader.GetUShort();
    }

    public void Serialize(NetDataWriter writer)
    {
      writer.Put(Id);
      writer.Put((byte)MoveInput);
      Utils.Vector3Writer.Serialise(writer, LookNormal);
      writer.Put(ServerTick);
    }
  }

}