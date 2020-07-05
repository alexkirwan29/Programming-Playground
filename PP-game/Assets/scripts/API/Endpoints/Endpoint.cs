  using PP.API;
  
namespace PP.API.Endpoints
{
  public class Endpoint
  {
    internal Controller controller;

    public Endpoint(Controller controller)
    {
      this.controller = controller;
    }
  }
}