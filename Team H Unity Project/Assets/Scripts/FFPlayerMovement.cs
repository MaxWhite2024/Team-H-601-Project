using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FFPlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float timeBetweenGridSteps;

    [Header("Movement Options")]
    [SerializeField] private bool isGridMovement = false;
    [SerializeField] private bool canStep = false;
    [SerializeField] private bool canAttackMove = true;

    [Header("Attack Settings")]
    [SerializeField] private float meleeTime;
    [SerializeField] private float rangedCooldown;

    [Header("Gameobjects and Components")]
    [SerializeField] private GameObject melee;
    [SerializeField] private GameObject ranged;
    [SerializeField] private GameObject characterCenter;

    private Vector2 p1MoveDir, p2MoveDir, characterMoveDir;
    private float p1Attack, p2Attack; //timer the melee hitbox appears and cooldown timer for ranged attacks
    
    //componment vars
    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
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
            Instantiate(ranged, transform.position, transform.rotation);
        }
    }

    void FixedUpdate()
    {
        #region Move Logic

        if((!canAttackMove && p1Attack <= 0) || canAttackMove)
        {
            //if players move via grid,...
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

                //if players have made new movement inputs,...
                if (characterMoveDir != Vector2.zero)
                {
                    //change direction of character
                    //Debug.Log("cur rot is : " + transform.rotation + ". and Euler move direction is: " + Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, characterMoveDir)));
                    transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, characterMoveDir));
                    // //if player should face a new direction,...
                    // if (transform.rotation != Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, characterMoveDir)))
                    // {
                    //     //change direction of character
                    //     transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, characterMoveDir));

                    //     //Wait 1 step
                    //     //StartCoroutine(DelayBoolean(hasTurned, timeBetweenGridSteps));
                    // }
                    // //else player is already facing correct direction,...
                    // else
                    // {
                    //     //set hasTurned to true
                    //     hasTurned = true;
                    // }

                    //if character can step,...
                    if (canStep)
                    {
                        //begin step cooldown
                        StartCoroutine(DelayStep(timeBetweenGridSteps));

                        //apply characterMoveDir to player
                        rb.AddForce(characterMoveDir * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
                    }
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

                //apply characterMoveDir to player
                rb.AddForce(characterMoveDir * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);

                //if players have made new movement inputs,...
                if (characterMoveDir != Vector2.zero)
                {
                    //change direction of character
                    transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, characterMoveDir));
                }
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

    IEnumerator DelayStep(float delayTime)
    {
        //Set canStep to false
        canStep = false;

        //wait for delayTime seconds
        yield return new WaitForSeconds(delayTime);

        //Set canStep to true
        canStep = true;
    }
}