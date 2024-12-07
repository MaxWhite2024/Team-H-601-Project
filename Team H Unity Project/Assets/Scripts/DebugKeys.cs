using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugKeys : MonoBehaviour
{
    [Header("Houses (Scenes) to Teleport to")]
    [SerializeField] private List<string> housesToTeleportTo = new List<string>();

    [Header("Component Variables")]
    [SerializeField] private Damageable playerDamageable;
    [SerializeField] private PlayerMovementAndAttack playerMovementAndAttack;
    private HouseManager houseManager;

    //integer variables
    private int startingPlayerArmor;
    private float startingAmmoRechargeTime;
    private float originalTimeScale;

    void Start()
    {
        //find houseManager script
        houseManager = (HouseManager)FindFirstObjectByType(typeof(HouseManager));
        if (!houseManager)
        {
            Debug.LogWarning("Debug Keys could not find the 'HouseManager' script. Please add the 'HouseManager' script to an GameObject");
        }

        //get startingPlayerArmor
        startingPlayerArmor = playerDamageable.armor;

        //get startingAmmoRechargeSpeed
        startingAmmoRechargeTime = playerMovementAndAttack.ammoRechargeTime;

        //get originalTimeScale
        originalTimeScale = Time.timeScale;
    }

    //when player presses '1',...
    void OnDebugTeleport1()
    {
        //reset timeScale
        Time.timeScale = originalTimeScale;

        //if first level in the housesToTeleportTo list exists,...
        if (housesToTeleportTo[0] != null)
        {
            //load first level
            SceneManager.LoadScene(housesToTeleportTo[0].ToString());
        }
    }

    //when player presses '2',...
    void OnDebugTeleport2()
    {
        //reset timeScale
        Time.timeScale = originalTimeScale;

        //if second level in the housesToTeleportTo list exists,...
        if (housesToTeleportTo[1] != null)
        {
            //load second level
            SceneManager.LoadScene(housesToTeleportTo[1].ToString());
        }
    }

    //when player presses '3',...
    void OnDebugTeleport3()
    {
        //reset timeScale
        Time.timeScale = originalTimeScale;

        //if third level in the housesToTeleportTo list exists,...
        if (housesToTeleportTo[2] != null)
        {
            //load third level
            SceneManager.LoadScene(housesToTeleportTo[2].ToString());
        }
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
        //kill everything in the current room
        houseManager.activeRoom.DebugClear();
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
        houseManager.activeRoom.DebugCleanRoom();
    }

    //when the player presses 'u'
    void OnDebugUnlimitedAmmo()
    {
        //toggle instant ammo recharge 
        playerMovementAndAttack.hasInstantAmmoRecharge = !playerMovementAndAttack.hasInstantAmmoRecharge;
    }
}
