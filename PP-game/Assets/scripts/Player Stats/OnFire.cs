using UnityEngine;

public class OnFire : PlayerStats_2 {
    public new static int StatID = 0;
    public float expiryTimer;
    public float damagePerSecond;
    
    public override void Inherited_Update(PlayerStats_2 returnScript) {
        for (int i = 0; i < returnScript.stats.Count; i++) {
            PlayerStats_2 ps_2 = returnScript.stats[i];
            if ((ps_2.StatID == StatID) && (ps_2 != this))
                returnScript.stats.Remove(ps_2);
        }

        expiryTimer -= Time.deltaTime;
        if (expiryTimer <= 0)
            returnScript.stats.Remove(this);

        returnScript.health -= damagePerSecond * Time.deltaTime;
    }
}