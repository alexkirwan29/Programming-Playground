using UnityEngine;
using LiteNetLib;

namespace PP.Networking {
  public abstract class NetController : MonoBehaviour {

    internal Networker net;
    
    /// <summary>
    /// Gets the ID of this controller.
    /// </summary>
    /// <returns></returns>
    public abstract byte GetId();

    /// <summary>
    /// Initialises this controller.
    /// </summary>
    /// <param name="networker">The networker to use.</param>
    public virtual void Init(Networker networker) {
      net = networker;
    }

    /// <summary>
    /// Invoked when the networker is shutting down.
    /// </summary>
    public virtual void Shutdown() {

    }
    
    /// <summary>
    /// Invoked when the networker has received data with the ID of this controller.
    /// </summary>
    /// <param name="reader">The reader for the incoming data.</param>
    public virtual void Read(NetPacketReader reader, NetPeer peer) {
      reader.Clear();
    }
  }
}