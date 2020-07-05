using PP.API;
using Newtonsoft.Json;

namespace PP.API.Objects
{
  public class Item : APIObject
  {
    [JsonProperty("item_id")]
    public uint Id { get; set; }    

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("picture_location")]
    public string PictureURL { get; set; }

    [JsonProperty("owner_id")]
    public uint? OwnerId { get; set; }
  }
}