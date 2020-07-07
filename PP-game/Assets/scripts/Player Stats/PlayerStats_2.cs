using System.Collections.Generic;
using UnityEngine;

public class PlayerStats_2 : ScriptableObject {
    public List<PlayerStats_2> stats = new List<PlayerStats_2>();
    private List<PlayerStats_2> prevStats = new List<PlayerStats_2>();

    public float health;    //Didn't want this as a stat that can be added and removed, so it's just hardcoded

    public int StatID { get; set; }
    public virtual void ListWasUpdated(PlayerStats_2 returnScript) { }
    public virtual void Inherited_Start(PlayerStats_2 returnScript) { }
    public virtual void Inherited_Awake(PlayerStats_2 returnScript) { }
    public virtual void Inherited_Update(PlayerStats_2 returnScript) { }

    public void Start() {
        for (int i = 0; i < stats.Count; i++) {
            stats[i].Inherited_Start(this);
        }
    }

    public void Awake() {
        for (int i = 0; i < stats.Count; i++) {
            stats[i].Inherited_Awake(this);
        }
    }

    public void Update() {
        bool changeRecorded = false;
        for (int i = 0; i < stats.Count; i++) {
            if (stats[i] != prevStats[i]) {
                changeRecorded = true;
                break;
            }
        }
        
        for (int i = 0; i < stats.Count; i++) {
            if (changeRecorded)
                stats[i].ListWasUpdated(this);
            stats[i].Inherited_Update(this);
        }
    }
}