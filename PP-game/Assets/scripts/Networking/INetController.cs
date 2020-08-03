namespace PP.Networking {
  public interface INetController {
    void Init(Networker networker);
    void Shutdown();
    bool Ready { get; }
  }
}