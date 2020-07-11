// using LiteNetLib;
namespace PP.Networking.Packets
{
  public class ChatMessage
  {
    public string message {get; set;}
    
    public ChatMessage() {}
    public ChatMessage(string message) { this.message = message; }
  }
}