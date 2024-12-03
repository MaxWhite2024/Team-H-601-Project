using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugKeys : MonoBehaviour
{
    [Header("Houses to Teleport Too")]
    [SerializeField] private List<Scene> housesToTeleportTo = new List<Scene>();

    [Header("")]
    [SerializeField] private Damageable playerDamageable;
    [SerializeField] private PlayerMovementAndAttack playerMovementAndAttack;
    private HouseManager houseManager;

    //integer variables
    private int startingPlayerArmor;
    private float startingAmmoRechargeTime;

    void Start()
    {
        //find houseManager script
        houseManager = (HouseManager)FindFirstObjectByType(typeof(HouseManager));
        if (!houseManager)
            Debug.LogWarning("Debug Keys could not find the 'HouseManager' script. Please add the 'HouseManager' script to an GameObject");

        //get startingPlayerArmor
        startingPlayerArmor = playerDamageable.armor;

        //get startingAmmoRechargeSpeed
        startingAmmoRechargeTime = playerMovementAndAttack.ammoRechargeTime;
    }

    //when player presses 'i',...
    void OnDebugInvul()
    {
        //if player is vulnerable,...
        if(playerDamageable.armor != int.MaxValue)
        {
            //make player invulnerable
            playerDamageable.armor = int.MaxValue;
        }
        //else player is invulnerable
        else
        {
            //make player vulnerable
            playerDamageable.armor = startingPlayerArmor;
        }
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
        playerDamageable.HealPlayer();
    }

    //when the player presses 'o',...
    void OnDebugOpen()
    {
        //open all doors in current room
    }

    //when the player presses 'u'
    void OnDebugUnlimitedAmmo()
    {
        //toggle instant ammo recharge 
        playerMovementAndAttack.hasInstantAmmoRecharge = !playerMovementAndAttack.hasInstantAmmoRecharge;
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
