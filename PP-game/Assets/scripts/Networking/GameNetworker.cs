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
    { get {
      return netMan.IsRunning;
    }}
    internal NetManager netMan;

    public NetPacketProcessor packetProcessor;

    internal abstract void Create();
    internal abstract void Destroy();

    internal virtual void DoUpdate(float dTime)
    {

    }
    public virtual void Awake()
    {
      DontDestroyOnLoad(gameObject);
      Create();
    }

    private void Update()
    {
      DoUpdate(Time.deltaTime);
    }

    private void OnDestroy()
    {
      Destroy();
    }

    public virtual void SendPacket<T>(T packet)
    {

    }
  }
}