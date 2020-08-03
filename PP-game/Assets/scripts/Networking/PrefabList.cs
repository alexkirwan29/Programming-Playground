using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PP.Networking;

namespace PP {

  [CreateAssetMenu(menuName = "Programming Playground/Prefab List")]
  public class PrefabList : ScriptableObject {

    public List<Info> Prefabs;

    [System.Serializable]
    public class Info {
      public NetworkedEntity Prefab;
      public ushort Id;
    }

    /// <summary>
    /// Get all the prefabs in this list as a Dictionary.
    /// </summary>
    /// <returns>All the prefabs.</returns>
    public Dictionary<ushort, NetworkedEntity> GetPrefabs() {
      var dict = new Dictionary<ushort, NetworkedEntity>();

      foreach (var item in Prefabs) {
        dict.Add(item.Id, item.Prefab);
      }

      return dict;
    }
  }
}