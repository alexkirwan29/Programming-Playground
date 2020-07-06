using UnityEngine;

public class Underwater : PlayerStats_2 {
    public new static int StatID = 3;
    public float damagePerSecond;
    
    public float drowningTimer;
    public float drowningThreshold;
    public float breathOutSpeed;
    public float breathInSpeed;
    public bool isUnderwater;

    public override void Inherited_Update(PlayerStats_2 returnScript) {
        for (int i = 0; i < returnScript.stats.Count; i++) {
            PlayerStats_2 ps_2 = returnScript.stats[i];
            if (((ps_2.StatID == StatID) || (ps_2.StatID == 0)) && (ps_2 != null))
                returnScript.stats.Remove(ps_2);
        }

        float temp = isUnderwater ? breathOutSpeed : -breathInSpeed;
        drowningTimer = Mathf.Clamp(drowningTimer + temp * Time.deltaTime, 0, drowningThreshold);

        if (drowningTimer == drowningThreshold)
            returnScript.health -= damagePerSecond * Time.deltaTime;
    }
}