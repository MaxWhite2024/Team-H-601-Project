using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementAndAttack : MonoBehaviour
{
    [Header("DEBUG VARIABLES")]
    [SerializeField] private bool inSyncMove = false;
    [SerializeField] private Vector2 characterMoveDir;
    [SerializeField] private Vector2 p1MoveDir, p2MoveDir;
    [SerializeField] private bool p1IsAttacking = false, p2IsAttacking = false;

    [Header("Movement Options")]
    [SerializeField] private bool canAttackMove = true;

    [Header("Movement Settings")]
    [SerializeField] private float freeFormMoveSpeed;
    [SerializeField] private float outOfSyncMovementPenalty;
    [SerializeField] private float movementInputBufferTimeLimit;
    private float tempP1MovementInputBuffertime = 0f, tempP2MovementInputBuffertime = 0f;
    private bool p1TappedMovement = false, p2TappedMovement = false;
    private bool isP1BufferTimerOn = false, isP2BufferTimerOn = false;
    private bool isP1TurningToNewDirection = false, isP2TurningToNewDirection = false;

    [Header("Attack Settings")]
    [SerializeField] private float fireRate = 0.3f;
    private float tempP1FireRate = 0f, tempP2FireRate = 0f, tempSyncedFireRate = 0f;
    private bool isP1Attacking = false, isP2Attacking = false;
    [SerializeField] private int maxAmmo;
    [HideInInspector] public int p1Ammo, p2Ammo;
    public float ammoRechargeTime;
    [SerializeField] private float movementAmmoRechargeMultiplier;
    [HideInInspector] public float tempP1AmmoRechargeTime = 0f, tempP2AmmoRechargeTime = 0f;
    [SerializeField] private float attackBufferTime;
    private float tempP1AttackBufferTime = 0f, tempP2AttackBufferTime = 0f;

    [Header("Gameobjects and Components")]
    [SerializeField] private GameObject meleeSwipe;
    [SerializeField] private GameObject p1MeleeStab;
    [SerializeField] private GameObject p2MeleeStab;
    public GameObject characterCenter;
    [SerializeField] private GameObject p1Center;
    [SerializeField] private GameObject p2Center;
    [SerializeField] private GameObject p1Projectile;
    [SerializeField] private GameObject p2Projectile;
    [SerializeField] private GameObject syncedProjectile;
    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //set players' move directions to zero
        p1MoveDir = Vector2.zero;
        p2MoveDir = Vector2.zero;
        characterMoveDir = Vector2.zero;

        //give each player max ammo
        p1Ammo = maxAmmo;
        p2Ammo = maxAmmo;
    }

    //when p1 pressed WASD,...
    void OnP1Move(InputValue value)
    {
        //set p1MoveDir to direction of WASD
        //NOTE: vector is already normalized!
        p1MoveDir = RemoveDiagonal(value.Get<Vector2>());

        //if p1MoveDir is not equal to zero,...
        if (p1MoveDir != Vector2.zero)
        {
            //if 
            if (p1MoveDir == EulerAngleToVector2(p1Center.transform.localEulerAngles.z))
            {
                isP1TurningToNewDirection = false;
            }
            else
            {
                isP1TurningToNewDirection = true;
            }

            //change direction of p2Center
            p1Center.transform.localEulerAngles = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, p1MoveDir));
        }
    }

    //when p2 pressed Arrow Keys,...
    void OnP2Move(InputValue value)
    {
        //set p2MoveDir to direction of WASD
        //NOTE: vector is already normalized!
        p2MoveDir = RemoveDiagonal(value.Get<Vector2>());

        //if p2MoveDir is not equal to zero,...
        if (p2MoveDir != Vector2.zero)
        {
            //Debug.Log(p2MoveDir == EulerAngleToVector2(p2Center.transform.localEulerAngles.z));
            //if 
            if(p2MoveDir == EulerAngleToVector2(p2Center.transform.localEulerAngles.z))
            {
                isP2TurningToNewDirection = false;
            }
            else
            {
                isP2TurningToNewDirection = true;
            }

            //change direction of p2Center
            p2Center.transform.localEulerAngles = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, p2MoveDir));
        }
    }

    //when p1 presses any p1 attack button,...
    void OnP1Attack()
    {
        //if p1 can fire again,...
        if (!p1IsAttacking && tempP1FireRate >= fireRate && p1Ammo > 0)
        {
            //set p1IsAttacking to true
            p1IsAttacking = true;

            //reset tempP1FireRate
            tempP1FireRate = 0f;
        }
    }

    //when p2 presses any p2 attack button,...
    void OnP2Attack()
    {
        //if p2 can fire again,...
        if (!p2IsAttacking && tempP2FireRate >= fireRate && p2Ammo > 0)
        {
            //set p2IsAttacking to true
            p2IsAttacking = true;

            //reset tempP2FireRate
            tempP2FireRate = 0f;
        }
    }

    void FixedUpdate()
    {
        ////***** Calculate if players have tapped or are holding down movement inputs *****
        #region Movement Input Buffering Logic

        //***** Check if p1 has tapped movement input *****
        //if p1MoveDir equals zero,...
        if (p1MoveDir == Vector2.zero)
        {
            //stop timer
            isP1BufferTimerOn = false;

            //p1TappedMovement is false
            p1TappedMovement = false;
        }
        //else p1MoveDir does NOT equal zero,...
        else
        {
            //if new p1 movement input direction is NOT facing towards the direction p1 is already facing,...
            if (isP1TurningToNewDirection)
            {
                //if timer is on,...
                if (isP1BufferTimerOn)
                {
                    //increment timer
                    tempP1MovementInputBuffertime += Time.fixedDeltaTime;
                }
                //else timer is off,...
                else
                {
                    //start timer
                    tempP1MovementInputBuffertime = 0f;
                    isP1BufferTimerOn = true;
                }

                //if movementInputBufferTimeLimit has NOT elapsed since timer has begun,...
                if (tempP1MovementInputBuffertime <= movementInputBufferTimeLimit)
                {
                    //set p1TappedMovement to true
                    p1TappedMovement = true;
                }
                //else movementInputBufferTimeLimit has elapsed since timer has begun,...
                else
                {
                    //set p1TappedMovement to false
                    p1TappedMovement = false;
                }
            }
            //else new p1 movement input direction is facing towards the direction p1 is already facing,...
            else
            {
                //stop timer
                isP1BufferTimerOn = false;

                //p1TappedMovement is false
                p1TappedMovement = false;
            }
        }

        //***** Check if p2 has tapped movement input *****
        //if p2MoveDir equals zero,...
        if (p2MoveDir == Vector2.zero)
        {
            //stop timer
            isP2BufferTimerOn = false;

            //p2TappedMovement is false
            p2TappedMovement = false;
        }
        //else p2MoveDir does NOT equal zero,...
        else
        {
            //if new p2 movement input direction is facing towards the direction p2 is already facing,...
            if (isP2TurningToNewDirection)
            {
                //if timer is on,...
                if (isP2BufferTimerOn)
                {
                    //increment timer
                    tempP2MovementInputBuffertime += Time.fixedDeltaTime;
                }
                //else timer is off,...
                else
                {
                    //start timer
                    tempP2MovementInputBuffertime = 0f;
                    isP2BufferTimerOn = true;
                }

                //if movementInputBufferTimeLimit has NOT elapsed since timer has begun,...
                if (tempP2MovementInputBuffertime <= movementInputBufferTimeLimit)
                {
                    //set p2TappedMovement to true
                    p2TappedMovement = true;
                }
                //else movementInputBufferTimeLimit has elapsed since timer has begun,...
                else
                {
                    //set p2TappedMovement to false
                    p2TappedMovement = false;
                }
            }
            //else new p2 movement input direction is NOT facing towards the direction p2 is already facing,...
            else
            {
                //stop timer
                isP2BufferTimerOn = false;

                //p2TappedMovement is false
                p2TappedMovement = false;
            }
        }

        //if (p2TappedMovement)
        //    Debug.Log("P2 TAPPED");
        //if (p1TappedMovement)
        //    Debug.Log("P1 TAPPED");

        #endregion

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
            //Debug.Log(characterMoveDir);

            //if p1 has tapped movement,...
            if(p1TappedMovement)
            {
                //set characterMoveDir to vector towards p1Center.transform.localEulerAngles.z
                Vector2 tempVector2 = EulerAngleToVector2(p1Center.transform.localEulerAngles.z);
                characterMoveDir = new Vector2(tempVector2.x, tempVector2.y);
                //Debug.Log(characterMoveDir);

                //***** Handle Character Rotation *****
                RotateCharacter();
            }
            //else if p2 has tapped movement,...
            else if (p2TappedMovement)
            {
                //set characterMoveDir to vector towards p2Center.transform.localEulerAngles.z
                Vector2 tempVector2 = EulerAngleToVector2(p2Center.transform.localEulerAngles.z);
                characterMoveDir = new Vector2(tempVector2.x, tempVector2.y);
                //Debug.Log(characterMoveDir);

                //***** Handle Character Rotation *****
                RotateCharacter();
            }
            //else neither p1 nor p2 have tapped movement,...
            else
            {
                //if players have made new movement inputs,...
                if (characterMoveDir != Vector2.zero)
                {
                    //***** Handle Free Form Movement *****
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
                        rb.AddForce(characterMoveDir * (freeFormMoveSpeed * outOfSyncMovementPenalty) * Time.fixedDeltaTime, ForceMode2D.Force);
                    }

                    #endregion

                    //***** Handle Character Rotation *****
                    RotateCharacter();
                }
            }
        }

        #endregion

        //***** Handle Ammo Recharge *****
        #region Ammo Recharge

        //***** Increment tempP1AmmoRechargeTime *****
        //if p1 is moving,...
        if (p1MoveDir != Vector2.zero)
        {
            //increment tempP1AmmoRechargeTime by movementAmmoRechargeMultiplier times fixedDeltaTime
            tempP1AmmoRechargeTime += movementAmmoRechargeMultiplier * Time.fixedDeltaTime;
        }
        //else p1 is NOT moving,...
        else
        {
            //increment tempP1AmmoRechargeTime by fixedDeltaTime
            tempP1AmmoRechargeTime += Time.fixedDeltaTime;
        }

        //***** Increment tempP2AmmoRechargeTime *****
        //if p2 is moving,...
        if (p2MoveDir != Vector2.zero)
        {
            //increment tempP2AmmoRechargeTime by movementAmmoRechargeMultiplier times fixedDeltaTime
            tempP2AmmoRechargeTime += movementAmmoRechargeMultiplier * Time.fixedDeltaTime;
        }
        //else p2 is NOT moving,...
        else
        {
            //increment tempP2AmmoRechargeTime by fixedDeltaTime
            tempP2AmmoRechargeTime += Time.fixedDeltaTime;
        }

        //***** Recharge P1 ammo *****
        //if p1's ammo is less than maxAmmo,...
        if (p1Ammo < maxAmmo)
        {
            //if ammoRechargeTime has elapsed,...
            if (tempP1AmmoRechargeTime >= ammoRechargeTime)
            {
                //increment p1Ammo by 1
                p1Ammo++;

                //reset tempP1AmmoRechargeTime
                tempP1AmmoRechargeTime = 0f;
            }
        }
        //else p1's ammo is greater than or equal to maxAmmo,...
        else
        {
            //keep tempP1AmmoRechargeTime at 0
            tempP1AmmoRechargeTime = 0f;
        }

        //***** Recharge P2 ammo *****
        //if p2's ammo is less than maxAmmo,...
        if (p2Ammo < maxAmmo)
        {
            //if ammoRechargeTime has elapsed,...
            if (tempP2AmmoRechargeTime >= ammoRechargeTime)
            {
                //increment p2Ammo by 1
                p2Ammo++;

                //reset tempP2AmmoRechargeTime
                tempP2AmmoRechargeTime = 0f;
            }
        }
        //else p2's ammo is greater than or equal to maxAmmo,...
        else
        {
            //keep tempP2AmmoRechargeTime at 0
            tempP2AmmoRechargeTime = 0f;
        }

        #endregion

        //***** Handle character attacking ******
        #region Attack Logic

        //increment tempP1FireRate and tempP2FireRate
        tempP1FireRate += Time.fixedDeltaTime;
        tempP2FireRate += Time.fixedDeltaTime;

        //if p1 is attacking,...
        if (p1IsAttacking)
        {
            //increment tempP1AttackBufferTime
            tempP1AttackBufferTime += Time.fixedDeltaTime;

            //if buffer time has elapsed,...
            if (tempP1AttackBufferTime >= attackBufferTime)
            {
                FireP1Projectile();
            }
            //else buffer time has not elapsed and p2 is also attacking in the same direction,...
            else if (p2IsAttacking && p1Center.transform.rotation == p2Center.transform.rotation)
            {
                FireSyncedProjectile();
            }
        }

        //if p2 is attacking,...
        if(p2IsAttacking)
        {
            //increment tempP2AttackBufferTime
            tempP2AttackBufferTime += Time.fixedDeltaTime;

            //if buffer time has elapsed,...
            if (tempP2AttackBufferTime >= attackBufferTime)
            {
                FireP2Projectile();
            }
            //else buffer time has not elapsed and p1 is also attacking in the same direction,...
            else if (p1IsAttacking && p1Center.transform.rotation == p2Center.transform.rotation)
            {
                FireSyncedProjectile();
            }
        }

        //increment attack timers
        tempP1FireRate += Time.fixedDeltaTime;
        tempP2FireRate += Time.fixedDeltaTime;
        tempSyncedFireRate += Time.fixedDeltaTime;

        #endregion
    }

    public Vector2 EulerAngleToVector2(float eulerAngle)
    {
        switch (eulerAngle)
        {
            case 0f:
                return Vector2.up;
            case 90f:
                return Vector2.left;
            case 180f:
                return Vector2.down;
            case 270f:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    private void RotateCharacter()
    {
        // Debug.Log("Current rotation: " + characterCenter.transform.localEulerAngles + ". Move Angle: " + AdjustedAngle(Vector2.SignedAngle(Vector2.up, characterMoveDir)));

        //create a Vector3 to store the rotation of movement direction in Euler angles
        Vector3 moveVec3 = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, characterMoveDir));

        //create a Vector3 to store adjusted current character rotation
        Vector3 currentRotation = new Vector3(characterCenter.transform.localEulerAngles.x, characterCenter.transform.localEulerAngles.y, AdjustedAngle(characterCenter.transform.localEulerAngles.z));

        //if players entered a new movement direction
        if (currentRotation != moveVec3)
        {
            // Debug.Log("Players want to move in a new direction!");
            //change direction of character
            characterCenter.transform.localEulerAngles = moveVec3;
        }
    }

    private void FireP1Projectile()
    {
        //fire p1 projectile in p1 direction
        Instantiate(p1Projectile, p1Center.transform.position, p1Center.transform.rotation);

        //decrement p1Ammo
        p1Ammo--;

        //set p1IsAttacking to false
        p1IsAttacking = false;

        //reset tempP1AttackBufferTime
        tempP1AttackBufferTime = 0f;
    }

    private void FireP2Projectile()
    {
        //fire p2 projectile in p2 direction
        Instantiate(p2Projectile, p2Center.transform.position, p2Center.transform.rotation);

        //decrement p2Ammo
        p2Ammo--;

        //set p2IsAttacking to false
        p2IsAttacking = false;

        //reset tempP2AttackBufferTime
        tempP2AttackBufferTime = 0f;
    }

    private void FireSyncedProjectile()
    {
        //fire a synced projectile in p1 direction
        Instantiate(syncedProjectile, p1Center.transform.position, p1Center.transform.rotation);

        //decrement p1Ammo and p2Ammo
        p1Ammo--;
        p2Ammo--;

        //set p1IsAttacking and p2IsAttacking to false
        p1IsAttacking = false;
        p2IsAttacking = false;

        //rest tempP1AttackBufferTime and tempP1AttackBufferTime
        tempP1AttackBufferTime = 0f;
        tempP2AttackBufferTime = 0f;
    }

    //***** Remove Diagonals from a Vector2 *****
    private Vector2 RemoveDiagonal(Vector2 inputVector)
    {
        float X = Mathf.Round(inputVector.x);
        float Y = Mathf.Round(inputVector.y);
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
}