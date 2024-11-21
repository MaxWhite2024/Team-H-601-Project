using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovementAndAttack;

public class PlayerSpriteManager : MonoBehaviour
{
    [Header("Sprite Renderer Variables")]
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private SpriteRenderer p1ArmSpriteRenderer;
    [SerializeField] private SpriteRenderer p2ArmSpriteRenderer;

    [Header("Component and GameObject Vairables")]
    [SerializeField] private PlayerMovementAndAttack playerMovementAndAttack;
    [SerializeField] private GameObject characterCenter;
    [SerializeField] private GameObject p1Center;
    [SerializeField] private GameObject p2Center;

    [Header("Player Sprite Variables")]
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    [Header("P1 Arm Variables")]
    [SerializeField] private Sprite p1Down;
    [SerializeField] private Sprite p1Up;
    [SerializeField] private Sprite p1Left;
    [SerializeField] private Sprite p1Right;

    [Header("P2 Arm Variables")]
    [SerializeField] private Sprite p2Down;
    [SerializeField] private Sprite p2Up;
    [SerializeField] private Sprite p2Left;
    [SerializeField] private Sprite p2Right;

    [Header("P1 Pip Variables")]
    private Color fadedP1PipColor;
    [SerializeField] private SpriteRenderer p1Pip1Renderer;
    [SerializeField] private SpriteRenderer p1Pip2Renderer;
    [SerializeField] private SpriteRenderer p1Pip3Renderer;

    [Header("P2 Pip Variables")]
    private Color fadedP2PipColor;
    [SerializeField] private SpriteRenderer p2Pip1Renderer;
    [SerializeField] private SpriteRenderer p2Pip2Renderer;
    [SerializeField] private SpriteRenderer p2Pip3Renderer;

    // Start is called before the first frame update
    void Start()
    {
        //set player sprite to front sprite
        playerSpriteRenderer.sprite = downSprite;

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
        //Debug.Log(playerMovementAndAttack.characterCenter.transform.localEulerAngles);

        //compare the z rotation of the character's center object to determine the character's sprite
        switch (characterCenter.transform.localEulerAngles.z)
        {
            //character is facing up
            case 0f:
                //change character sprite to upSprite
                playerSpriteRenderer.sprite = upSprite;
                break;

            //character is facing down
            case 180f:
                //change character sprite to downSprite
                playerSpriteRenderer.sprite = downSprite;
                break;

            //character is facing left
            case 90f:
                //change character sprite to leftSprite
                playerSpriteRenderer.sprite = leftSprite;
                break;

            //character is facing right
            case 270f:
                //change character sprite to rightSprite
                playerSpriteRenderer.sprite = rightSprite;
                break;

            //character is facing up and right
            case -45f:
                //change character sprite to rightSprite
                playerSpriteRenderer.sprite = rightSprite;
                break;

            //character is facing up and left
            case 45f:
                //change character sprite to leftSprite
                playerSpriteRenderer.sprite = leftSprite;
                break;

            //character is facing down and left
            case 135f:
                //change character sprite to leftSprite
                playerSpriteRenderer.sprite = leftSprite;
                break;

            //character is facing down and right
            case -135f:
                //change character sprite to rightSprite
                playerSpriteRenderer.sprite = rightSprite;
                break;
        }

        //compare the z rotation of P1's center object to determine the P1's arm sprite
        switch (p1Center.transform.localEulerAngles.z)
        {
            //P1 Arm is facing up
            case 0f:
                //change P1 Arm sprite to p1Up
                p1ArmSpriteRenderer.sprite = p1Up;
                break;

            //P1 Arm is facing down
            case 180f:
                //change P1 Arm sprite to downSprite
                p1ArmSpriteRenderer.sprite = p1Down;
                break;

            //P1 Arm is facing left
            case 90f:
                //change P1 Arm sprite to p1Left
                p1ArmSpriteRenderer.sprite = p1Left;
                break;

            //P1 Arm is facing right
            case 270f:
                //change P1 Arm sprite to p1Right
                p1ArmSpriteRenderer.sprite = p1Right;
                break;
        }

        //compare the z rotation of P2's center object to determine the P2's arm sprite
        switch (p2Center.transform.localEulerAngles.z)
        {
            //P2 Arm is facing up
            case 0f:
                //change P2 Arm sprite to p2Up
                p2ArmSpriteRenderer.sprite = p2Up;
                break;

            //P2 Arm is facing down
            case 180f:
                //change P2 Arm sprite to downSprite
                p2ArmSpriteRenderer.sprite = p2Down;
                break;

            //P2 Arm is facing left
            case 90f:
                //change P2 Arm sprite to p2Left
                p2ArmSpriteRenderer.sprite = p2Left;
                break;

            //P2 Arm is facing right
            case 270f:
                //change P2 Arm sprite to p2Right
                p2ArmSpriteRenderer.sprite = p2Right;
                break;
        }

        //compare P1's ammo
        switch (playerMovementAndAttack.p1Ammo)
        {
            //P1 was 0 ammo
            case 0:
                //fade all 3 pips
                p1Pip1Renderer.color = fadedP1PipColor;
                p1Pip2Renderer.color = fadedP1PipColor;
                p1Pip3Renderer.color = fadedP1PipColor;
                break;

            //P1 was 1 ammo
            case 1:
                //intensify lower pip
                p1Pip1Renderer.color = Color.white;

                //fade 2 higher pips
                p1Pip2Renderer.color = fadedP1PipColor;
                p1Pip3Renderer.color = fadedP1PipColor;
                break;

            //P1 was 2 ammo
            case 2:
                //intensify 2 lower pips
                p1Pip1Renderer.color = Color.white;
                p1Pip2Renderer.color = Color.white;

                //fade 1 higher pip
                p1Pip3Renderer.color = fadedP1PipColor;
                break;

            //P1 was 3 ammo
            case 3:
                //intensify all 3 pips
                p1Pip1Renderer.color = Color.white;
                p1Pip2Renderer.color = Color.white;
                p1Pip3Renderer.color = Color.white;
                break;
        }

        //compare P2's ammo
        switch (playerMovementAndAttack.p2Ammo)
        {
            //P2 was 0 ammo
            case 0:
                //fade all 3 pips
                p2Pip1Renderer.color = fadedP2PipColor;
                p2Pip2Renderer.color = fadedP2PipColor;
                p2Pip3Renderer.color = fadedP2PipColor;
                break;

            //P1 was 1 ammo
            case 1:
                //intensify lower pip
                p2Pip1Renderer.color = Color.white;

                //fade 2 higher pips
                p2Pip2Renderer.color = fadedP2PipColor;
                p2Pip3Renderer.color = fadedP2PipColor;
                break;

            //P1 was 2 ammo
            case 2:
                //intensify 2 lower pips
                p2Pip1Renderer.color = Color.white;
                p2Pip2Renderer.color = Color.white;

                //fade 1 higher pip
                p2Pip3Renderer.color = fadedP2PipColor;
                break;

            //P1 was 3 ammo
            case 3:
                //intensify all 3 pips
                p2Pip1Renderer.color = Color.white;
                p2Pip2Renderer.color = Color.white;
                p2Pip3Renderer.color = Color.white;
                break;
        }
    }
}