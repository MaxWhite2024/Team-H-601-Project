using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FFPlayerMovement : MonoBehaviour
{
    [Header("DEBUG VARIABLES")]
    [SerializeField] private bool hasTurned = true;
    [SerializeField] private bool canStep = false;
    [SerializeField] private bool inSyncMove = false;

    [Header("Movement Options")]
    [SerializeField] private bool isGridMovement = false;
    [SerializeField] private bool canAttackMove = true;

    [Header("Free Form Movement Settings")]
    [SerializeField] private float freeFormMoveSpeed;

    [Header("Grid Movement Settings")]
    [SerializeField] private float gridMoveSpeed;
    [SerializeField] private float timeBetweenGridSteps;

    [Header("Attack Settings")]
    [SerializeField] private float meleeTime;
    [SerializeField] private float rangedCooldown;

    [Header("Gameobjects and Components")]
    [SerializeField] private GameObject melee;
    [SerializeField] private GameObject ranged;
    [SerializeField] private GameObject characterCenter;

    private Vector2 p1MoveDir, p2MoveDir, characterMoveDir;
    private float p1Attack, p2Attack; //timer the melee hitbox appears and cooldown timer for ranged attacks
    private float tempTimeBetweenGridSteps = 0f;
    
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
        //increment tempTimeBetweenGridSteps
        tempTimeBetweenGridSteps += Time.fixedDeltaTime;

        //if timeBetweenGridSteps has elapsed,...
        if(tempTimeBetweenGridSteps >= timeBetweenGridSteps)
        {
            //set canStep to true 
            canStep = true;
        }

        #region Move Syncronization Logic

        //if both p1 and p2 are non-zero and moving in the same direction,...
        if(p1MoveDir != Vector2.zero && p2MoveDir != Vector2.zero && p1MoveDir == p2MoveDir)
        {
            //set inSyncMove to true
            inSyncMove = true;
        }
        //else players are out of sync so,...
        else
        {
            //set inSyncMove to false
            inSyncMove = false;
        }

        #endregion

        #region Move Logic

        if ((!canAttackMove && p1Attack <= 0) || canAttackMove)
        {
            //if players move via grid,...
            if (isGridMovement)
            {
                #region Grid Movement

                //calculate grid characterMoveDir
                if (inSyncMove)
                {
                    characterMoveDir = p1MoveDir;
                }
                else
                {
                    characterMoveDir = Vector2.zero;
                }

                //if players have made new movement inputs,...
                if (characterMoveDir != Vector2.zero)
                {         
                    //create a Vector3 to store the rotation of movement direction in Euler angles
                    // Debug.Log("Current rotation: " + characterCenter.transform.localEulerAngles + ". Move Angle: " + AdjustedAngle(Vector2.SignedAngle(Vector2.up, characterMoveDir)));
                    Vector3 moveVec3 = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, characterMoveDir));

                    //create a Vector3 to store adjusted current character rotation
                    Vector3 currentRotation = new Vector3(characterCenter.transform.localEulerAngles.x, characterCenter.transform.localEulerAngles.y, AdjustedAngle(characterCenter.transform.localEulerAngles.z));

                    //if players entered a new movement direction
                    if(currentRotation != moveVec3)
                    {
                        //Debug.Log("Players want to move in a new direction!");

                        //wait 1 step
                        canStep = false;
                        tempTimeBetweenGridSteps = 0f;

                        //change direction of character
                        characterCenter.transform.localEulerAngles = moveVec3;
                    }

                    //if character can step,...
                    if (canStep)
                    {
                        // Debug.Log("Players can step!");

                        //apply characterMoveDir to player
                        rb.AddForce(characterMoveDir * gridMoveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);

                        //wait 1 step
                        canStep = false;
                        tempTimeBetweenGridSteps = 0f;
                    }
                }

                #endregion
            }
            else
            {
                #region Free Form Movement

                //calculate free form characterMoveDir
                if (p1MoveDir == p2MoveDir)
                {
                    characterMoveDir = p1MoveDir * 2f;
                }
                else
                {
                    characterMoveDir = Vector2.zero;
                }

                //apply characterMoveDir to player
                rb.AddForce(characterMoveDir * freeFormMoveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);

                //if players have made new movement inputs,...
                if (characterMoveDir != Vector2.zero)
                {
                    //change direction of character
                    transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, characterMoveDir));
                }

                #endregion
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

    public float AdjustedAngle(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        }

        return angle;
    }
}