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

    // Start is called before the first frame update
    void Start()
    {
        //set player sprite to front sprite
        playerSpriteRenderer.sprite = downSprite;
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
    }
}
