using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerAmmoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text p1AmmoText;
    [SerializeField] private TMP_Text p2AmmoText;
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
        P1AmmoRechargeSlider.value = playerMovementAndAttack.tempP1AmmoRechargeTime;
        P2AmmoRechargeSlider.value = playerMovementAndAttack.tempP2AmmoRechargeTime;
    }
}
