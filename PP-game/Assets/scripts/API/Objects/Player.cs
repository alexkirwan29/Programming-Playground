using System;
using Newtonsoft.Json;

namespace PP.API.Objects
{
  public class Player : APIObject
  {
    // The ID of this player.
    [JsonProperty("player_id")]
    public uint Id { get; set; }

    // The username of this player.
    [JsonProperty("username")]
    public string UserName { get; set; }

    // The inventory ID of this player.
    [JsonProperty("inv_id")]
    public uint? InventoryId { get; set; }

    // The last login of this player.
    [JsonProperty("last_login")]
    public DateTime? LastLogin { get; set; }
  }
}