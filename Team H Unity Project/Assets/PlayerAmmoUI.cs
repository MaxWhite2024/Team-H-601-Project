using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerAmmoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text p1AmmoText;
    [SerializeField] private TMP_Text p2AmmoText;
    [SerializeField] private TMP_Text p1ModeText;
    [SerializeField] private TMP_Text p2ModeText;
    [SerializeField] private Slider P1AmmoRechargeSlider;
    [SerializeField] private Slider P2AmmoRechargeSlider;
    private PlayerMovementAndAttack playerMovementAndAttack;


    // Start is called before the first frame update
    void Start()
    {
        playerMovementAndAttack = GameObject.Find("Players").GetComponent<PlayerMovementAndAttack>();
        P1AmmoRechargeSlider.maxValue = playerMovementAndAttack.ammoRechargeTime;
        P2AmmoRechargeSlider.maxValue = playerMovementAndAttack.ammoRechargeTime;
    }

    // Update is called once per frame
    void Update()
    {
        p1AmmoText.text = "P1 Ammo: " + playerMovementAndAttack.p1Ammo;
        p2AmmoText.text = "P2 Ammo: " + playerMovementAndAttack.p2Ammo;
        p1ModeText.text = "P1 Mode: " + PlayerMode.GetName(typeof(PlayerMode), (int)playerMovementAndAttack.p1Mode);
        p2ModeText.text = "P2 Mode: " + PlayerMode.GetName(typeof(PlayerMode), (int)playerMovementAndAttack.p2Mode);
        P1AmmoRechargeSlider.value = playerMovementAndAttack.tempP1AmmoRechargeTime;
        P2AmmoRechargeSlider.value = playerMovementAndAttack.tempP2AmmoRechargeTime;
    }
}
