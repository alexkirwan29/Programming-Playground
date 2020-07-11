using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LiteNetLib;
using LiteNetLib.Utils;
using PP.Networking;
using System.Net;
using System.Net.Sockets;

namespace PP.Networking
{
  public abstract class GameNetworker : MonoBehaviour
  {
    public bool IsRunning
    {
      get
      {
        return netMan.IsRunning;
      }
    }
    
    internal NetManager netMan;
    internal NetDataWriter cachedWriter;

    private bool HasInit = false;

    public NetPacketProcessor packetProcessor;

    internal abstract void Create();
    internal abstract void Destroy();

    internal virtual void DoUpdate(float dTime)
    {

    }
    public virtual void StartNetworker()
    {
      DontDestroyOnLoad(gameObject);

      packetProcessor = new NetPacketProcessor();
      cachedWriter = new NetDataWriter();

      HasInit = true;

      Create();

      NetChat.Initialise(this);
    }

    private void Update()
    {
      if(HasInit)
        DoUpdate(Time.deltaTime);

      if(netMan != null && netMan.IsRunning)
        netMan.PollEvents();
    }

    private void OnDestroy()
    {
      if(HasInit)
      {
        NetChat.DeInitialise();
      }

      Destroy();
    }

    internal virtual NetDataWriter WriteSerialisable<T>(T packet) where T: struct, INetSerializable
    {
      cachedWriter.Reset();
      packet.Serialize(cachedWriter);
      return cachedWriter;
    }
    internal virtual NetDataWriter WritePacket<T>(T packet) where T : class, new()
    {
      cachedWriter.Reset();
      packetProcessor.Write(cachedWriter, packet);
      return cachedWriter;
    }
  }
}