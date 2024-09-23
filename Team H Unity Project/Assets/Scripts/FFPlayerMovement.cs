using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FFPlayerMovement : MonoBehaviour
{
    //number vars
    [SerializeField] private float moveSpeed, timeBetweenGridSteps, meleeTime, rangedCooldown;
    [SerializeField] private bool isGridMovement = false, canStep = true, canAttackMove = true;
    [SerializeField] private GameObject melee, ranged;
    private Vector2 p1MoveDir, p2MoveDir, characterMoveDir;
    private float p1Attack, p2Attack; //timer the melee hitbox appears and cooldown timer for ranged attacks
    
    //componment vars
    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent is expensive so I assigned rb in the inspector :)
        //get rgidbody2D of player
        //rb = GetComponent<Rigidbody2D>();

        //set players' move directions to zero
        p1MoveDir = Vector2.zero;
        p2MoveDir = Vector2.zero;
        characterMoveDir = Vector2.zero;

        //sets attacking variables to 0
        p1Attack = 0.0f;
        p2Attack = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, p1MoveDir.normalized * 100f, Color.red);
        Debug.DrawRay(transform.position, p2MoveDir.normalized * 100f, Color.blue);
    }

    //when p1 pressed WASD,...
    void OnP1Move(InputValue value)
    {
        //set p1MoveDir to direction of WASD
        //NOTE: vector is already normalized!
        p1MoveDir = RemoveDiagonal(value.Get<Vector2>());
    }

    //when p2 pressed Arrow Keys,...
    void OnP2Move(InputValue value)
    {
        //set p2MoveDir to direction of Arrow Keys
        //NOTE: vector is already normalized!
        p2MoveDir = RemoveDiagonal(value.Get<Vector2>());
    }

    void OnP1Attack()
    {
        if(p1Attack > 0)
        {
            p1Attack += meleeTime;
        }
        else
        {
            p1Attack = meleeTime;
        }
        //Debug.Log(p1Attack);
        melee.SetActive(true);
    }

    void OnP2Attack()
    {
        if(p2Attack <= 0)
        {
            p2Attack = rangedCooldown;
            //Instantiate(ranged, (transform.position + (transform.up /1.7f)), transform.rotation); Spawns the projecile in front of the player
            Instantiate(ranged, transform.position, transform.rotation);
        }
    }

    void FixedUpdate()
    {
        Debug.Log(transform.rotation);
        #region Move Logic

        if((!canAttackMove && p1Attack <= 0) || canAttackMove)
        {
            if (isGridMovement)
            {
                //calculate grid characterMoveDir
                if (p1MoveDir == p2MoveDir)
                {
                    characterMoveDir = p1MoveDir * 2f;
                }
                else
                {
                    characterMoveDir = Vector2.zero;
                }

                //if character can step,...
                if (canStep)
                {
                    //apply characterMoveDir to player
                    rb.AddForce(characterMoveDir * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);

                    //begin step cooldown
                    StartCoroutine(GridStepDelay(timeBetweenGridSteps));
                }
            }
            else
            {
                //calculate grid characterMoveDir
                if (p1MoveDir == p2MoveDir)
                {
                    characterMoveDir = p1MoveDir * 2f;
                }
                else
                {
                    characterMoveDir = Vector2.zero;
                }
                //calculate freeform characterMoveDir
                // characterMoveDir = p1MoveDir + p2MoveDir;

                //apply characterMoveDir to player
                rb.AddForce(characterMoveDir * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
            }

            //if players have made new movement inputs,...
            if (characterMoveDir != Vector2.zero)
            {
                //change direction of character
                transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, characterMoveDir));
            }
        }

        #endregion

        #region Attack Logic

        if (p1Attack > 0)
        {
            p1Attack -= Time.deltaTime;

            if (p1Attack <= 0)
            {
                melee.SetActive(false);
            }
        }

        if(p2Attack > 0)
        {
            p2Attack -= Time.deltaTime;
        }

        #endregion
    }

    private Vector2 RemoveDiagonal(Vector2 inputVector)
    {
        float X = inputVector.x;
        float Y = inputVector.y;
        if(X*X > Y*Y)
        {
            return new Vector2(X,0);
        } 
        else 
        {
            return new Vector2(0,Y);
        }
    }

    IEnumerator GridStepDelay(float delayTime)
    {
        //prevent character from stepping
        canStep = false;

        //wait for delayTime seconds
        yield return new WaitForSeconds(delayTime);

        //allow character to step
        canStep = true;
    }
}