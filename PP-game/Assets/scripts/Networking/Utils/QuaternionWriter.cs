using LiteNetLib.Utils;
using UnityEngine;


namespace PP.Networking.Utils {
  public static class QuaternionWriter {

    // Most of the code below is from Stag Point's gist. https://gist.github.com/StagPoint/bb7edf61c2e97ce54e3e4561627f6582
    // It has been adapted for me to understand a little bit more easily.

    private const float PRESISION = 32767f;

    public static void Serialise(NetDataWriter writer, Quaternion rotation) {

      // Find the largest value in the Quaternion and stores its index, value and sign.
      byte largestIndex = 0;
      float largestValue = float.MinValue;
      float sign = 1f;

      for (int i = 0; i < 4; i++) {
        var abs = Mathf.Abs(rotation[i]);

        if (abs > largestIndex) {
          if (rotation[i] < 0)
            sign = -1;
          else
            sign = 1;
        }

        largestIndex = (byte)i;
        largestValue = rotation[i];
      }

      // If one of the components is 1f we can reconstruct it at the other end. Send what
      // we know.
      if (Mathf.Approximately(largestValue, 1f)) {
        writer.Put(largestIndex + 4);
        return;
      }

      short a, b, c;

      // Send everything except for the largest component.
      if (largestIndex == 0) {
        a = (short)(rotation.y * sign * PRESISION);
        b = (short)(rotation.z * sign * PRESISION);
        c = (short)(rotation.w * sign * PRESISION);
      }
      else if (largestIndex == 1) {
        a = (short)(rotation.x * sign * PRESISION);
        b = (short)(rotation.z * sign * PRESISION);
        c = (short)(rotation.w * sign * PRESISION);
      }
      else if (largestIndex == 2) {
        a = (short)(rotation.x * sign * PRESISION);
        b = (short)(rotation.y * sign * PRESISION);
        c = (short)(rotation.w * sign * PRESISION);
      }
      else {
        a = (short)(rotation.x * sign * PRESISION);
        b = (short)(rotation.y * sign * PRESISION);
        c = (short)(rotation.z * sign * PRESISION);
      }

      writer.Put(largestIndex);
      writer.Put(a);
      writer.Put(b);
      writer.Put(c);
    }
    public static Quaternion Deserialise(NetDataReader reader) {

      byte largestIndex = reader.GetByte();

      // Sorry for showing you this mess, it simply sets whatever component is the largest to 1.
      // this works because quaternion math.
      if (largestIndex >= 4 && largestIndex <= 7) {
        return new Quaternion(
          (largestIndex == 4) ? 1f : 0f,
          (largestIndex == 5) ? 1f : 0f,
          (largestIndex == 6) ? 1f : 0f,
          (largestIndex == 7) ? 1f : 0f
        );
      }

      // Get the a b and c components that where sent over the network.
      float a = (float)reader.GetShort() / PRESISION;
      float b = (float)reader.GetShort() / PRESISION;
      float c = (float)reader.GetShort() / PRESISION;

      // Reconstruct the missing component. *again quaternion math*
      float d = Mathf.Sqrt(1f - (a * a + b * b + c * c));

      // Depending on what index we get, return with a correctly assembled quaternion.
      if (largestIndex == 0)
        return new Quaternion(d, a, b, c);

      if (largestIndex == 1)
        return new Quaternion(a, d, b, c);

      if (largestIndex == 2)
        return new Quaternion(a, b, d, c);

      if (largestIndex == 3)
        return new Quaternion(a, b, c, d);

      Debug.Log(largestIndex);
      
      throw new System.Exception("Failed to reconstruct a quaternion");
    }
  }
}
