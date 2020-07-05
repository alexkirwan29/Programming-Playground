using Newtonsoft.Json;

namespace PP.API.Objects
{
  public class Inventory : APIObject
  {
    [JsonProperty("inv_id")]
    public int Id { get; set; }

    // The name of the inventory.
    [JsonProperty("name")]
    public string Name { get; set; }

    // The maximum amount of item slots in this inventory.
    [JsonProperty("max_slots")]
    public string MaximumSlots { get; set; }

    // The slots in this inventory;
    [JsonProperty("slots")]
    public Slot[] Slots { get; set; }

    public class Slot
    {
      [JsonProperty("i")]
      public uint ItemId { get; set; }

      [JsonProperty("q")]
      public int Quantity { get; set; }
    }
  }
}