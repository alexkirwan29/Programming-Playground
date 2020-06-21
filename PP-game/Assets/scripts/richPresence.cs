using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class richPresence : MonoBehaviour
{

    public Discord.Discord discord;

  //  public float clientID;
      
    
    // Start is called before the first frame update
    void Start()
    {

        discord = new Discord.Discord(724144886135390250, (System.UInt64)Discord.CreateFlags.Default);
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            State = "Learning Discord Rich Presence",
            Details = "It's Easier Than I Thought",
            
        };

        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                Debug.LogError("Everything is fine!");
            }
        });

    }

    // Update is called once per frame
    void Update()
    {
        discord.RunCallbacks();
                
    }




}
