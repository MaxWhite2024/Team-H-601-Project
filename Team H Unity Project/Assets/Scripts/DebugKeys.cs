using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugKeys : MonoBehaviour
{
    private HouseManager houseManager;

    void Start()
    {
        //find houseManager script
        houseManager = (HouseManager)FindFirstObjectByType(typeof(HouseManager));
        if (!houseManager)
            Debug.LogWarning("Debug Keys could not find the 'HouseManager' script. Please add the 'HouseManager' script to an GameObject");
    }

    //when player presses 'i',...
    void OnDebugInvul()
    {
        //if player is already invulnerable,...
            //make player vulnerable
        //else player is vulnerable
            //make player invulnerable
    }

    //when player presses 'k',...
    void OnDebugKillEnemies()
    {
        //find the currently active room
        //houseManager.rooms.Find();

        //kill everything in the current room
        //houseManager.;
    }

    //when the player presses 'h',...
    void OnDebugHeal()
    {
        //heal player to max health
    }

    //when the player presses 'o',...
    void OnDebugOpen()
    {
        //open all doors in current room
    }

    //when the player presses 'u'
    void OnDebugUnlimitedAmmo()
    { 
        //set ammo recharge speed to 0f
    }


    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
