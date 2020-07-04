using UnityEditor;
using UnityEngine;

//This piece of code is literally just to stop all the public variables in PlayerStats from showing in the inspector
//[CustomEditor(typeof(PlayerStats))]
//public class PlayerStatsEditor : Editor {
//    public override void OnInspectorGUI() { }
//}

public class PlayerStats : MonoBehaviour {
    //Health Stats
    public float Health = 100;                                          // This is the players current health
    public float MaxHealth = 100;                                       // This is the players max health

    //Stamina Stats
    public float Stamina = 100;                                         // This is the players current stamina
    public float MaxStamina = 100;                                      // This is the players max stamina

    //Health Regen stats
    public float DefaultRegenerationSpeed = 2;                          // The current regeneration speed
    public float ExtraRegenSpeed = 0;                                   // Extra regen applied by another object
    public float ExtraRegenExpiry = 0;                                  // When the extra regen expires

    //Drowning Stats
    public bool IsUnderwater = false;                                   // Is the player underwater?
    public float DrowningThreshold = 10;                                // How many seconds until the player starts to drown
    public float DrowningAirTimer = 0;                                 // Internal timer variable for how long until you drown
    public float DrowningSpeed = 1;                                     // How fast the player drowns
    public float BreathInSpeed = 0.5f;                                  // How fast the player breaths in after drowning
    public float DamageFromDrowning = 10;                               // How much damage per frame the player takes while drowning

    //Poison Stats
    public bool IsPoisoned = false;                                     // Is the player poisoned?
    public float PoisonTimer = 0;                                      // Internal timer variable for how long until the effect wears off
    public float DamageFromPoison = 5;                                  // How much damage per frame the player takes while poisoned

    //Fire Stats
    public bool IsOnFire = false;                                       // Is the player on fire?
    public float FireTimer = 0;                                        // Internal timer variable for how long until the effect wears off
    public float DamageFromFire = 15;                                   // How much damage per frame the player takes while on fire

    void Update() {
        //Drowning Logic
        if (DrowningAirTimer == DrowningThreshold)
            Health -= DamageFromDrowning;           //If the player is at the drowning threshold, start taking damage

        if (IsUnderwater) {
            //If we are underwater, increase the drown timer to a maximum of the point at which the player starts to drown
            //The clamp prevents the player from drowning on land after spending time underwater
            DrowningAirTimer = Mathf.Clamp(DrowningAirTimer + Time.deltaTime * DrowningSpeed, 0, DrowningThreshold);
            IsOnFire = false;
            FireTimer = 0;
        } else
            //We are not underwater, so decrease the drown timer to zero
            DrowningAirTimer = Mathf.Clamp(DrowningAirTimer - Time.deltaTime * BreathInSpeed, 0, DrowningThreshold);
        
        //Poison Logic
        if (IsPoisoned) {
            //If we are poisoned, take health away from the player
            Health -= DamageFromPoison * Time.deltaTime;

            //Decrement the poison timer, and if it is smaller than zero, we are no longer poisoned
            PoisonTimer -= Time.deltaTime;
            if (PoisonTimer <= 0)
                IsPoisoned = false;
        }
        
        //On Fire Logic
        if (IsOnFire) {
            //If we are on fire, take health away from the player
            Health -= DamageFromFire * Time.deltaTime;

            //Decrement the fire timer, and if it is smaller than zero, we are no longer on fire
            FireTimer -= Time.deltaTime;
            if (FireTimer <= 0)
                IsOnFire = false;
        }

        //Health Regen Logic
        if (ExtraRegenExpiry > 0)
            Health = Mathf.Clamp(Health + (DefaultRegenerationSpeed + ExtraRegenSpeed) * Time.deltaTime, 0, MaxHealth);
        else
            Health = Mathf.Clamp(Health + DefaultRegenerationSpeed * Time.deltaTime, 0, MaxHealth);
        ExtraRegenExpiry--;
    }

    //Set the state of "IsUnderwater"
    public void SetUnderwater(bool underwater) { IsUnderwater = underwater; }

    //Set the state of "IsOnFire" and set the expiry timer
    public void SetOnFire(bool fireState, float expiryTimer) {
        IsOnFire = fireState;
        if (FireTimer < expiryTimer)
            FireTimer = expiryTimer;
    }

    //Set the state of "IsPoisoned" and set the expiry timer
    public void SetPoisoned(bool poisonState, float expiryTimer) {
        IsPoisoned = poisonState;
        if (PoisonTimer < expiryTimer)
            PoisonTimer = expiryTimer;
    }

    //Set the extra regen expiry timer
    public void SetRegen(float regenSpeed, float expiryTimer) {
        if (ExtraRegenExpiry * ExtraRegenSpeed < expiryTimer * regenSpeed)
            ExtraRegenExpiry = expiryTimer;
            ExtraRegenSpeed = regenSpeed;
    }
}