using System.Collections.Generic;
using UnityEngine;

public class PlayerStats_2_Interface : MonoBehaviour {
    [HideInInspector]
    public PlayerStats_2 ps2 = new PlayerStats_2();

    void Update() { ps2.Update(); }
}

public class PlayerStats_2 : ScriptableObject {
    List<PlayerStats_2> stats = new List<PlayerStats_2>();
    
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
        for (int i = 0; i < stats.Count; i++) {
            stats[i].Inherited_Update(this);
        }
    }
}

//public class Poisoned : ScriptableObject { }
public class OnFire : PlayerStats_2 {
    public string statName = "Fire";

    //Run this at the start of the scene
    public override void Inherited_Start(PlayerStats_2 returnScript) {
        
    }

    //Run this when the script is enabled
    public override void Inherited_Awake(PlayerStats_2 returnScript) {

    }

    //Run this every frame
    public override void Inherited_Update(PlayerStats_2 returnScript) {
        
    }
}