using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerAmmoUI : MonoBehaviour
{
    [Header("General Variables")]
    private PlayerMovementAndAttack playerMovementAndAttack;

    [Header("P1 Pip Variables")]
    [SerializeField] private Image p1Pip1Image;
    [SerializeField] private Image p1Pip2Image;
    [SerializeField] private Image p1Pip3Image;
    private Color fadedP1PipColor;
    private Vector3 p1ImageOriginalScale;

    [Header("P2 Pip Variables")]
    [SerializeField] private Image p2Pip1Image;
    [SerializeField] private Image p2Pip2Image;
    [SerializeField] private Image p2Pip3Image;
    private Color fadedP2PipColor;
    private Vector3 p2ImageOriginalScale;

    // Start is called before the first frame update
    void Start()
    {
        playerMovementAndAttack = GameObject.Find("Players").GetComponent<PlayerMovementAndAttack>();

        //define fadedP1PipColor as white with 0.2 alpha
        fadedP1PipColor = Color.white;
        fadedP1PipColor.a = 0.2f;

        //save orignal scale of p1 pip images
        p1ImageOriginalScale = p1Pip1Image.gameObject.transform.localScale;

        //define fadedP2PipColor as white with 0.2 alpha
        fadedP2PipColor = Color.white;
        fadedP2PipColor.a = 0.2f;

        //save orignal scale of p2 pip images
        p2ImageOriginalScale = p2Pip1Image.gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        #region P1 Ammo Images

        //compare P1's ammo
        switch (playerMovementAndAttack.p1Ammo)
        {
            //P1 was 0 ammo
            case 0:
                //fade all 3 pips
                p1Pip1Image.color = fadedP1PipColor;
                p1Pip2Image.color = fadedP1PipColor;
                p1Pip3Image.color = fadedP1PipColor;

                //scale up pip1 over time and shrink pip2 and pip3
                p1Pip1Image.gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, p1ImageOriginalScale, playerMovementAndAttack.tempP1AmmoRechargeTime / playerMovementAndAttack.ammoRechargeTime);
                p1Pip2Image.gameObject.transform.localScale = Vector3.zero;
                p1Pip3Image.gameObject.transform.localScale = Vector3.zero;

                break;

            //P1 was 1 ammo
            case 1:
                //intensify lower pip
                p1Pip1Image.color = Color.white;

                //fade 2 higher pips
                p1Pip2Image.color = fadedP1PipColor;
                p1Pip3Image.color = fadedP1PipColor;

                //scale up pip2 over time and shrink pip3
                p1Pip1Image.gameObject.transform.localScale = p1ImageOriginalScale;
                p1Pip2Image.gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, p1ImageOriginalScale, playerMovementAndAttack.tempP1AmmoRechargeTime / playerMovementAndAttack.ammoRechargeTime);
                p1Pip3Image.gameObject.transform.localScale = Vector3.zero;

                break;

            //P1 was 2 ammo
            case 2:
                //intensify 2 lower pips
                p1Pip1Image.color = Color.white;
                p1Pip2Image.color = Color.white;

                //fade 1 higher pip
                p1Pip3Image.color = fadedP1PipColor;

                //scale up pip3 over time
                p1Pip1Image.gameObject.transform.localScale = p1ImageOriginalScale;
                p1Pip2Image.gameObject.transform.localScale = p1ImageOriginalScale;
                p1Pip3Image.gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, p1ImageOriginalScale, playerMovementAndAttack.tempP1AmmoRechargeTime / playerMovementAndAttack.ammoRechargeTime);

                break;

            //P1 was 3 ammo
            case 3:
                //intensify all 3 pips
                p1Pip1Image.color = Color.white;
                p1Pip2Image.color = Color.white;
                p1Pip3Image.color = Color.white;

                //keep all pips at original scale
                p1Pip1Image.gameObject.transform.localScale = p1ImageOriginalScale;
                p1Pip2Image.gameObject.transform.localScale = p1ImageOriginalScale;
                p1Pip3Image.gameObject.transform.localScale = p1ImageOriginalScale;

                break;
        }

        #endregion

        #region P2 Ammo Images

        //compare P2's ammo
        switch (playerMovementAndAttack.p2Ammo)
        {
            //P2 was 0 ammo
            case 0:
                //fade all 3 pips
                p2Pip1Image.color = fadedP2PipColor;
                p2Pip2Image.color = fadedP2PipColor;
                p2Pip3Image.color = fadedP2PipColor;

                //scale up pip1 over time and shrink pip2 and pip3
                p2Pip1Image.gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, p2ImageOriginalScale, playerMovementAndAttack.tempP2AmmoRechargeTime / playerMovementAndAttack.ammoRechargeTime);
                p2Pip2Image.gameObject.transform.localScale = Vector3.zero;
                p2Pip3Image.gameObject.transform.localScale = Vector3.zero;

                break;

            //P2 was 1 ammo
            case 1:
                //intensify lower pip
                p2Pip1Image.color = Color.white;

                //fade 2 higher pips
                p2Pip2Image.color = fadedP2PipColor;
                p2Pip3Image.color = fadedP2PipColor;

                //scale up pip2 over time and shrink pip3
                p2Pip1Image.gameObject.transform.localScale = p2ImageOriginalScale;
                p2Pip2Image.gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, p2ImageOriginalScale, playerMovementAndAttack.tempP2AmmoRechargeTime / playerMovementAndAttack.ammoRechargeTime);
                p2Pip3Image.gameObject.transform.localScale = Vector3.zero;

                break;

            //P2 was 2 ammo
            case 2:
                //intensify 2 lower pips
                p2Pip1Image.color = Color.white;
                p2Pip2Image.color = Color.white;

                //fade 1 higher pip
                p2Pip3Image.color = fadedP2PipColor;

                //scale up pip3 over time
                p2Pip1Image.gameObject.transform.localScale = p2ImageOriginalScale;
                p2Pip2Image.gameObject.transform.localScale = p2ImageOriginalScale;
                p2Pip3Image.gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, p2ImageOriginalScale, playerMovementAndAttack.tempP2AmmoRechargeTime / playerMovementAndAttack.ammoRechargeTime);

                break;

            //P2 was 3 ammo
            case 3:
                //intensify all 3 pips
                p2Pip1Image.color = Color.white;
                p2Pip2Image.color = Color.white;
                p2Pip3Image.color = Color.white;

                //keep all pips at original scale
                p2Pip1Image.gameObject.transform.localScale = p2ImageOriginalScale;
                p2Pip2Image.gameObject.transform.localScale = p2ImageOriginalScale;
                p2Pip3Image.gameObject.transform.localScale = p2ImageOriginalScale;

                break;
        }

        #endregion
    }
}
