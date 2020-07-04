using UnityEngine;

public class PlayerStats : MonoBehaviour {
    public float Health;                // This is the players current health
    public float MaxHealth;             // This is the players max health

    public float Stamina;               // This is the players current stamina
    public float MaxStamina;            // This is the players max stamina

    public float Regeneration;          // The current regeneration speed

    public bool IsUnderwater = false;   // Is the player underwater?
    public float DrowningThreshold;     // How many seconds until the player starts to drown
    private float DrowningAirTimer;     // Internal timer variable for how long until you drown
    public float DrowningSpeed;         // How fast the player drowns
    public float BreathInSpeed;         // How fast the player breaths in after drowning
    public float DamageFromDrowing;     // How much damage per frame the player takes while drowning

    public bool IsPoisoned = false;     // Is the player poisoned?
    private float PoisonTimer;          // Internal timer variable for how long until the effect wears off
    public float DamageFromPoison;      // How much damage per frame the player takes while poisoned

    public bool IsOnFire = false;       // Is the player on fire?
    private float FireTimer;            // Internal timer variable for how long until the effect wears off
    public float DamageFromFire;        // How much damage per frame the player takes while on fire

    void Update() {
        if (DrowningAirTimer == DrowningThreshold)
            Health -= DamageFromDrowing;

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

        Health = Mathf.Clamp(Health + Regeneration, 0, MaxHealth);
    }

    void SetUnderwater(bool underwater) {
        IsUnderwater = underwater;

        if (!IsUnderwater) { DrowningAirTimer = 0; return; }
    }

    void SetOnFire(bool fireState) {
        IsOnFire = fireState;

        if (!IsOnFire) { FireTimer = 0; return; }
    }
}