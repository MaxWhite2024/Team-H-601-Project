using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerAmmoUI : MonoBehaviour
{
    [Header("General Variables")]
    [SerializeField] private TMP_Text p1AmmoText;
    [SerializeField] private TMP_Text p2AmmoText;
    [SerializeField] private Slider P1AmmoRechargeSlider;
    [SerializeField] private Slider P2AmmoRechargeSlider;
    private PlayerMovementAndAttack playerMovementAndAttack;

    [Header("P1 Pip Variables")]
    private Color fadedP1PipColor;
    [SerializeField] private Image p1Pip1Image;
    [SerializeField] private Image p1Pip2Image;
    [SerializeField] private Image p1Pip3Image;

    [Header("P2 Pip Variables")]
    private Color fadedP2PipColor;
    [SerializeField] private Image p2Pip1Image;
    [SerializeField] private Image p2Pip2Image;
    [SerializeField] private Image p2Pip3Image;

    // Start is called before the first frame update
    void Start()
    {
        playerMovementAndAttack = GameObject.Find("Players").GetComponent<PlayerMovementAndAttack>();
        P1AmmoRechargeSlider.maxValue = playerMovementAndAttack.ammoRechargeTime;
        P2AmmoRechargeSlider.maxValue = playerMovementAndAttack.ammoRechargeTime;

        //define fadedP1PipColor as white with 0.2 alpha
        fadedP1PipColor = Color.white;
        fadedP1PipColor.a = 0.2f;

        //define fadedP2PipColor as white with 0.2 alpha
        fadedP2PipColor = Color.white;
        fadedP2PipColor.a = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        //p1AmmoText.text = "P1 Ammo: " + playerMovementAndAttack.p1Ammo;
        //p2AmmoText.text = "P2 Ammo: " + playerMovementAndAttack.p2Ammo;
        P1AmmoRechargeSlider.value = playerMovementAndAttack.tempP1AmmoRechargeTime;
        P2AmmoRechargeSlider.value = playerMovementAndAttack.tempP2AmmoRechargeTime;

        //compare P1's ammo
        switch (playerMovementAndAttack.p1Ammo)
        {
            //P1 was 0 ammo
            case 0:
                //fade all 3 pips
                p1Pip1Image.color = fadedP1PipColor;
                p1Pip2Image.color = fadedP1PipColor;
                p1Pip3Image.color = fadedP1PipColor;
                break;

            //P1 was 1 ammo
            case 1:
                //intensify lower pip
                p1Pip1Image.color = Color.white;

                //fade 2 higher pips
                p1Pip2Image.color = fadedP1PipColor;
                p1Pip3Image.color = fadedP1PipColor;
                break;

            //P1 was 2 ammo
            case 2:
                //intensify 2 lower pips
                p1Pip1Image.color = Color.white;
                p1Pip2Image.color = Color.white;

                //fade 1 higher pip
                p1Pip3Image.color = fadedP1PipColor;
                break;

            //P1 was 3 ammo
            case 3:
                //intensify all 3 pips
                p1Pip1Image.color = Color.white;
                p1Pip2Image.color = Color.white;
                p1Pip3Image.color = Color.white;
                break;
        }

        //compare P2's ammo
        switch (playerMovementAndAttack.p2Ammo)
        {
            //P2 was 0 ammo
            case 0:
                //fade all 3 pips
                p2Pip1Image.color = fadedP2PipColor;
                p2Pip2Image.color = fadedP2PipColor;
                p2Pip3Image.color = fadedP2PipColor;
                break;

            //P1 was 1 ammo
            case 1:
                //intensify lower pip
                p2Pip1Image.color = Color.white;

                //fade 2 higher pips
                p2Pip2Image.color = fadedP2PipColor;
                p2Pip3Image.color = fadedP2PipColor;
                break;

            //P1 was 2 ammo
            case 2:
                //intensify 2 lower pips
                p2Pip1Image.color = Color.white;
                p2Pip2Image.color = Color.white;

                //fade 1 higher pip
                p2Pip3Image.color = fadedP2PipColor;
                break;

            //P1 was 3 ammo
            case 3:
                //intensify all 3 pips
                p2Pip1Image.color = Color.white;
                p2Pip2Image.color = Color.white;
                p2Pip3Image.color = Color.white;
                break;
        }
    }
}
