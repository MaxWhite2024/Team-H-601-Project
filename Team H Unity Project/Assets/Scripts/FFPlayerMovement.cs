using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FFPlayerMovement : MonoBehaviour
{
    [Header("DEBUG VARIABLES")]
    [SerializeField] private bool canStep = false;
    [SerializeField] private bool inSyncMove = false;
    [SerializeField] private bool inSyncAttack = false;
    [SerializeField] private Vector2 characterMoveDir;

    [Header("Movement Options")]
    [SerializeField] private bool isGridMovement = false;
    [SerializeField] private bool canAttackMove = true;

    [Header("Free Form Movement Settings")]
    [SerializeField] private float freeFormMoveSpeed;

    [Header("Grid Movement Settings")]
    [SerializeField] private float gridMoveSpeed;
    [SerializeField] private float inspectorTimeBetweenGridSteps;
    private float timeBetweenGridSteps;
    private float tempTimeBetweenGridSteps = 0f;

    [Header("Attack Options")]
    [SerializeField] private AttackType attackType; 

    [Header("Attack Settings")]
    [SerializeField] private float meleeTime;

    [Header("Gameobjects and Components")]
    [SerializeField] private GameObject meleeSwipe;
    [SerializeField] private GameObject meleeStab;
    [SerializeField] private GameObject characterCenter;

    private Vector2 p1MoveDir, p2MoveDir;
    private float curP1AttackTimer, curP2AttackTimer; //timers for melee hitboxes
    private bool isP1Attacking = false, isP2Attacking = false;
    
    //componment vars
    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //set players' move directions to zero
        p1MoveDir = Vector2.zero;
        p2MoveDir = Vector2.zero;
        characterMoveDir = Vector2.zero;

        //sets attacking timer variables to 0
        curP1AttackTimer = 0f;
        curP2AttackTimer = 0f;

        //set timeBetweenGridSteps to the value from inspector
        timeBetweenGridSteps = inspectorTimeBetweenGridSteps;
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
        //Debug.Log(p1Attack);
        if(attackType == AttackType.BOTH_MELEE)
        {
            if(curP1AttackTimer >= meleeTime)
            {
                //reset curP1AttackTimer
                curP1AttackTimer = 0f;

                //set isP1Attacking to true
                isP1Attacking = true;
            }
        }    
    }

    void OnP2Attack()
    {
        //Debug.Log(p2Attack);
        if(attackType == AttackType.BOTH_MELEE)
        {
            if(curP2AttackTimer >= meleeTime)
            {
                //reset curP2AttackTimer
                curP2AttackTimer = 0f;

                //set isP1Attacking to true
                isP1Attacking = true;
            }
        } 
    }

    void OnSwapMovementType()
    {
        //invert isGridMovement
        isGridMovement = !isGridMovement;
    }

    void OnNextAttackType()
    {
        //if the current attackType is greater than the number of attack types minus 1,...
        if ((int) attackType >= AttackType.GetNames(typeof(AttackType)).Length - 1)
        {
            //set attackType back to first attack type
            attackType = 0;
        }
        //else the current attackType is less than or equal to the number of attack types,...
        else
        {
            //increment attack type
            attackType++;
        }
        // Debug.Log(attackType);
    }

    void FixedUpdate()
    {
        //Debug.Log(transform.rotation);
        //increment tempTimeBetweenGridSteps
        tempTimeBetweenGridSteps += Time.fixedDeltaTime;

        //if timeBetweenGridSteps has elapsed,...
        if(tempTimeBetweenGridSteps >= timeBetweenGridSteps)
        {
            //set canStep to true 
            canStep = true;
        }

        //***** Calculate characterMoveDir and inSyncMove variables ******
        #region Move Syncronization Logic

        //if both p1 and p2 are both non-zero,...
        if (p1MoveDir != Vector2.zero && p2MoveDir != Vector2.zero)
        {
            //if both p1 and p2 are moving in the same direction,...
            if (p1MoveDir == p2MoveDir)
            {
                // Debug.Log("IN SYNC");
                //set inSyncMove to true
                inSyncMove = true;

                //set characterMoveDir to p1MoveDir since both players are moving in same direction
                characterMoveDir = p1MoveDir;
            }
            //else players are moving in different directions,...
            else
            {
                // Debug.Log("UNDECIDED");
                //set inSyncMove to false
                inSyncMove = false;

                //set characterMoveDir to zero
                characterMoveDir = Vector2.zero;
            }
        }
        //else neither player is moving so,...
        else
        {
            //set inSyncMove to false
            inSyncMove = false;

            //if p1 is moving and p2 is NOT moving,...
            if (p1MoveDir != Vector2.zero && p2MoveDir == Vector2.zero)
            {
                // Debug.Log("p1 MOVE");
                //set characterMoveDir to p1MoveDir
                characterMoveDir = p1MoveDir;

            }
            //else if p1 is NOT moving and p2 is moving,...
            else if (p1MoveDir == Vector2.zero && p2MoveDir != Vector2.zero)
            {
                // Debug.Log("p2 MOVE");
                //set characterMoveDir to p2MoveDir
                characterMoveDir = p2MoveDir;
            }
            else
            {
                // Debug.Log("NIETHER moving");
                //set inSyncMove to false
                inSyncMove = false;

                //set characterMoveDir to zero
                characterMoveDir = Vector2.zero;
            }
        }

        #endregion

        //***** Handle character movement and apply movement force to character *****
        #region Move and Rotation Logic

        // Debug.Log("p1 move dir is: " + p1MoveDir + ". p2 move dir is: " + p2MoveDir + ". chracter move direction is: " + characterMoveDir);
        if ((!canAttackMove && !isP1Attacking && !isP2Attacking) || canAttackMove)
        {
            //if players have made new movement inputs,...
            if (characterMoveDir != Vector2.zero)
            {
                //if players move via grid,...
                if (isGridMovement)
                {
                    //***** Handle grid movement ******
                    #region Grid Movement

                    //if character can step,...
                    if (canStep)
                    {
                        // Debug.Log("Players can step!");

                        //apply force in characterMoveDir to the character
                        rb.AddForce(characterMoveDir * gridMoveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);

                        //if players are moving in sync,...
                        if (inSyncMove)
                        {
                            //set timeBetweenGridSteps to the value from inspector
                            timeBetweenGridSteps = inspectorTimeBetweenGridSteps;

                            //wait 1 step
                            canStep = false;
                            tempTimeBetweenGridSteps = 0f;
                        }
                        //else players are NOT moving in sync,...
                        else
                        {
                            //set timeBetweenGridSteps to double the value from inspector so that the charcater must wait 2 steps
                            timeBetweenGridSteps = inspectorTimeBetweenGridSteps * 2f;

                            //wait 2 steps
                            canStep = false;
                            tempTimeBetweenGridSteps = 0f;
                        }
                    }

                    #endregion
                }
                else
                {
                    //****** Handle free form movement ******
                    #region Free Form Movement
                    
                    //players are moving in sync
                    if (inSyncMove)
                    {
                        //apply characterMoveDir to player
                        rb.AddForce(characterMoveDir * freeFormMoveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
                    }
                    else
                    {
                        //apply penalized characterMoveDir to player
                        rb.AddForce(characterMoveDir * (freeFormMoveSpeed / 2f) * Time.fixedDeltaTime, ForceMode2D.Force);
                    }

                    #endregion
                }

                #region Character Rotation Logic

                // Debug.Log("Current rotation: " + characterCenter.transform.localEulerAngles + ". Move Angle: " + AdjustedAngle(Vector2.SignedAngle(Vector2.up, characterMoveDir)));

                //create a Vector3 to store the rotation of movement direction in Euler angles
                Vector3 moveVec3 = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, characterMoveDir));

                //create a Vector3 to store adjusted current character rotation
                Vector3 currentRotation = new Vector3(characterCenter.transform.localEulerAngles.x, characterCenter.transform.localEulerAngles.y, AdjustedAngle(characterCenter.transform.localEulerAngles.z));

                //if players entered a new movement direction
                if (currentRotation != moveVec3)
                {
                    // Debug.Log("Players want to move in a new direction!");

                    //if using grid movement,...
                    if(isGridMovement)
                    {
                        //wait 1 step
                        canStep = false;
                        tempTimeBetweenGridSteps = 0f;
                    }

                    //change direction of character
                    characterCenter.transform.localEulerAngles = moveVec3;
                }

                #endregion
            }
        }

        #endregion

        //***** Handle character attacking ******
        #region Attack Logic

        //if both p1 and p2 are attacking,...
        if(isP1Attacking && isP2Attacking)
        {
            //deactivate melee stab
            meleeStab.SetActive(false);

            //activate melee swipe
            meleeSwipe.SetActive(true);
        }
        //else if p1 is attacking and p2 is NOT attacking,...
        else if(isP1Attacking && !isP2Attacking)
        {
            //activate melee stab
            meleeStab.SetActive(true);
        }
        //else if p1 is NOT attacking and p2 is attacking,...
        else if(!isP1Attacking && isP2Attacking)
        {
            //activate melee stab
            meleeStab.SetActive(true);
        }

        if(curP1AttackTimer >= meleeTime)
        {
            //
            isP1Attacking = false;

            //
            meleeStab.SetActive(false);

            //
            meleeSwipe.SetActive(false);
        }

        if(curP2AttackTimer >= meleeTime)
        {
            //
            isP2Attacking = false;

            //
            meleeStab.SetActive(false);

            //
            meleeSwipe.SetActive(false);
        }

        //increment attack timers
        curP1AttackTimer += Time.fixedDeltaTime;
        curP2AttackTimer += Time.fixedDeltaTime;

        #endregion
    }

    //***** Remove Diagonals from a Vector2 *****
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

    //***** Use alchemy and magic to take in a angle in degrees and convert it to an angle that transform.rotation can use *****
    public float AdjustedAngle(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        }

        return angle;
    }

    private enum AttackType
    {
        MELEE_AND_RANGED, BOTH_MELEE, BOTH_RANGED
    }
}