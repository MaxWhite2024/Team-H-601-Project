using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovementAndAttack;

public class PlayerSpriteManager : MonoBehaviour
{
    [Header("Component Variables")]
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private PlayerMovementAndAttack playerMovementAndAttack;
    [SerializeField] private SpriteRenderer p1ArrowSpriteRenderer;
    [SerializeField] private SpriteRenderer p2ArrowSpriteRenderer;

    [Header("Player Sprite Variables")]
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    [Header("Arrow Variables")]
    [SerializeField] private Sprite p1MovementSprite;
    [SerializeField] private Sprite p2MovementSprite;
    [SerializeField] private Sprite p1AttackSprite;
    [SerializeField] private Sprite p2AttackSprite;

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

        //compare the z rotation of the character's center object
        switch (playerMovementAndAttack.characterCenter.transform.localEulerAngles.z)
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

        ////compare the mode of p1
        //switch (playerMovementAndAttack.p1Mode)
        //{
        //    //p1 is in MOVE mode
        //    case PlayerMode.MOVE:
        //        //change p1 arrow sprite to p1MovementSprite
        //        p1ArrowSpriteRenderer.sprite = p1MovementSprite;
        //        break;

        //    //p1 is in ATTACK mode
        //    case PlayerMode.ATTACK:
        //        //change p2 arrow sprite to p1AttackSprite
        //        p1ArrowSpriteRenderer.sprite = p1AttackSprite;
        //        break;
        //}

        ////compare the mode of p2
        //switch (playerMovementAndAttack.p2Mode)
        //{
        //    //p1 is in MOVE mode
        //    case PlayerMode.MOVE:
        //        //change p2 arrow sprite to p1MovementSprite
        //        p2ArrowSpriteRenderer.sprite = p2MovementSprite;
        //        break;
        //    //p2 is in ATTACK mode
        //    case PlayerMode.ATTACK:
        //        //change p2 arrow sprite to p2AttackSprite
        //        p2ArrowSpriteRenderer.sprite = p2AttackSprite;
        //        break;
        //}
    }
}
