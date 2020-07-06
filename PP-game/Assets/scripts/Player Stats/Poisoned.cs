using UnityEngine;

public class Poisoned : PlayerStats_2 {
    public new static int StatID = 1;
    public float expiryTimer;
    public float damagePerSecond;
    
    public override void Inherited_Update(PlayerStats_2 returnScript) {
        expiryTimer -= Time.deltaTime;
        if (expiryTimer <= 0)
            returnScript.stats.Remove(this);

        returnScript.health -= damagePerSecond * Time.deltaTime;
    }
}