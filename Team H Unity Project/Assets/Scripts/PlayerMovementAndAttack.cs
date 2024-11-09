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
                //***** Handle grid movement ******
                #region Grid Movement

                //if character can step,...
                //if (canStep)
                //{
                //    // Debug.Log("Players can step!");

                //    //apply force in characterMoveDir to the character
                //    rb.AddForce(characterMoveDir * gridMoveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);

                //    //if players are moving in sync,...
                //    if (inSyncMove)
                //    {
                //        //set timeBetweenGridSteps to the value from inspector
                //        timeBetweenGridSteps = inspectorTimeBetweenGridSteps;

                //        //wait 1 step
                //        canStep = false;
                //        tempTimeBetweenGridSteps = 0f;
                //    }
                //    //else players are NOT moving in sync,...
                //    else
                //    {
                //        //set timeBetweenGridSteps to double the value from inspector so that the charcater must wait outOfSyncMovementPenalty steps
                //        timeBetweenGridSteps = inspectorTimeBetweenGridSteps * (1 / outOfSyncMovementPenalty);

                //        //wait outOfSyncMovementPenalty steps
                //        canStep = false;
                //        tempTimeBetweenGridSteps = 0f;
                //    }
                //}

                #endregion

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

                    //wait 1 step
                    //canStep = false;
                    //tempTimeBetweenGridSteps = 0f;

                    //change direction of character
                    characterCenter.transform.localEulerAngles = moveVec3;
                }

                #endregion
            }
        }

        #endregion

        //***** Handle Ammo Recharge *****
        #region Ammo Recharge

        //*****  *****
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