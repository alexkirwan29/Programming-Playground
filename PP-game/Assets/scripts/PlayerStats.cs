using UnityEngine;
using System.Collections.Generic;

public struct StatBase {
    string StatName;

}

public class PlayerStats : MonoBehaviour {
    //Health Stats
    public float Health;                                                // This is the players current health
    public float MaxHealth;                                             // This is the players max health

    //Stamina Stats
    public float Stamina;                                               // This is the players current stamina
    public float MaxStamina;                                            // This is the players max stamina

    public float DefaultRegeneration;                                   // The current regeneration speed
    public List<StatBase> Regeneration_Effects = new List<StatBase>();

    public bool IsUnderwater = false;                                   // Is the player underwater?
    public float DrowningThreshold;                                     // How many seconds until the player starts to drown
    private float DrowningAirTimer;                                     // Internal timer variable for how long until you drown
    public float DrowningSpeed;                                         // How fast the player drowns
    public float BreathInSpeed;                                         // How fast the player breaths in after drowning
    public float DamageFromDrowning;                                    // How much damage per frame the player takes while drowning

    public bool IsPoisoned = false;                                     // Is the player poisoned?
    private float PoisonTimer;                                          // Internal timer variable for how long until the effect wears off
    public float DamageFromPoison;                                      // How much damage per frame the player takes while poisoned

    public bool IsOnFire = false;                                       // Is the player on fire?
    private float FireTimer;                                            // Internal timer variable for how long until the effect wears off
    public float DamageFromFire;                                        // How much damage per frame the player takes while on fire

    void Update() {
        if (DrowningAirTimer == DrowningThreshold)
            Health -= DamageFromDrowning;
        //Health = DrowningAirTimer == DrowningThreshold ? Health -= DamageFromDrowning : Health;

        if (IsUnderwater) {
            DrowningAirTimer = Mathf.Clamp(DrowningAirTimer + Time.deltaTime * DrowningSpeed, 0, DrowningThreshold);
            IsOnFire = false;
            FireTimer = 0;
        } else
            DrowningAirTimer = Mathf.Clamp(DrowningAirTimer - Time.deltaTime * BreathInSpeed, 0, DrowningThreshold);
        
        if (IsPoisoned) {
            Health -= DamageFromPoison;
            PoisonTimer = Mathf.Clamp(PoisonTimer - Time.deltaTime,0,Mathf.Infinity);
            if (PoisonTimer == 0)
                IsPoisoned = false;
        }
        
        if (IsOnFire) {
            Health -= DamageFromFire;
            FireTimer = Mathf.Clamp(FireTimer - Time.deltaTime, 0, Mathf.Infinity);
            if (FireTimer == 0)
                IsOnFire = false;
        }

        Health = Mathf.Clamp(Health + DefaultRegeneration, 0, MaxHealth);
    }

    void SetUnderwater(bool underwater) {
        IsUnderwater = underwater;
        if (!IsUnderwater) { DrowningAirTimer = 0; return; }
    }

    void SetOnFire(bool fireState, float expiryTimer) {
        IsOnFire = fireState;
        if (!IsOnFire) { FireTimer = expiryTimer; return; }
    }

    void SetPoisoned(bool poisonState, float expiryTimer) {
        IsPoisoned = poisonState;
        if (!IsPoisoned) { PoisonTimer = expiryTimer; return; }
    }
}