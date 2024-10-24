using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementAndAttack : MonoBehaviour
{
    [Header("DEBUG VARIABLES")]
    [SerializeField] private bool canStep = false;
    [SerializeField] private bool inSyncMove = false;
    [SerializeField] private bool inSyncAttack = false;
    [SerializeField] private Vector2 characterMoveDir;
    [SerializeField]
    private bool isP1Attacking = false, isP2Attacking = false;
    public PlayerMode p1Mode = PlayerMode.MOVE;
    public PlayerMode p2Mode = PlayerMode.MOVE;
    [SerializeField] private Vector2 p1MoveDir, p2MoveDir;
    [SerializeField] private Vector2 p1AttackDir, p2AttackDir;

    [Header("Movement Options")]
    [SerializeField] private bool canAttackMove = true;

    [Header("Movement Settings")]
    [SerializeField] private float gridMoveSpeed;
    [SerializeField] private float inspectorTimeBetweenGridSteps;
    [SerializeField] private float outOfSyncMovementPenalty;
    private float timeBetweenGridSteps;
    private float tempTimeBetweenGridSteps = 0f;
    
    [Header("Attack Settings")]
    [SerializeField] private float fireRate = 0.3f;
    private float tempP1FireRate = 0f, tempP2FireRate = 0f, tempSyncedFireRate = 0f;
    [SerializeField] private int maxAmmo;
    public int p1Ammo, p2Ammo;
    [SerializeField] private float ammoRechargeTime;
    private float tempP1AmmoRechargeTime = 0f, tempP2AmmoRechargeTime = 0f;

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

    //componment vars
    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //set players' move directions to zero
        p1MoveDir = Vector2.zero;
        p2MoveDir = Vector2.zero;
        characterMoveDir = Vector2.zero;

        //set players' attack directions to zero
        p1AttackDir = Vector2.zero;
        p2AttackDir = Vector2.zero;

        //set timeBetweenGridSteps to the value from inspector
        timeBetweenGridSteps = inspectorTimeBetweenGridSteps;

        //give each player max ammo
        p1Ammo = maxAmmo;
        p2Ammo = maxAmmo;
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
        //if p1's mode is move,...
        if (p1Mode == PlayerMode.MOVE)
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

            //set p1AttackDir to zero
            p1AttackDir = Vector2.zero;
        }
        //else p1's mode is attack,...
        else
        {
            //set p1AttackDir to direction of WASD
            //NOTE: vector is already normalized!
            p1AttackDir = RemoveDiagonal(value.Get<Vector2>());

            //if p1AttackDir is not equal to zero,...
            if (p1AttackDir != Vector2.zero)
            {
                //change direction of p1Center
                p1Center.transform.localEulerAngles = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, p1AttackDir));
            }

            //set p1MoveDir to zero
            p1MoveDir = Vector2.zero;
        }
    }

    //when p2 pressed Arrow Keys,...
    void OnP2Move(InputValue value)
    {
        //if p2's mode is move,...
        if (p2Mode == PlayerMode.MOVE)
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

            //set p2AttackDir to zero
            p2AttackDir = Vector2.zero;
        }
        //else p2's mode is attack,...
        else
        {
            //set p1AttackDir to direction of WASD
            //NOTE: vector is already normalized!
            p2AttackDir = RemoveDiagonal(value.Get<Vector2>());

            //if p2AttackDir is not equal to zero,...
            if (p2AttackDir != Vector2.zero)
            {
                //change direction of p2Center
                p2Center.transform.localEulerAngles = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, p2AttackDir));
            }

            //set p2MoveDir to zero
            p2MoveDir = Vector2.zero;
        }
    }

    void OnP1Attack()
    {
        //if the current p1Mode is greater than the number of attack types minus 1,...
        if ((int)p1Mode >= PlayerMode.GetNames(typeof(PlayerMode)).Length - 1)
        {
            //set p1Mode back to first attack type
            p1Mode = 0;
        }
        //else the current p1Mode is less than or equal to the number of attack types,...
        else
        {
            //increment attack type
            p1Mode++;
        }
        // Debug.Log(p1Mode);
    }

    void OnP2Attack()
    {
        //if the current p2Mode is greater than the number of attack types minus 1,...
        if ((int)p2Mode >= PlayerMode.GetNames(typeof(PlayerMode)).Length - 1)
        {
            //set p2Mode back to first attack type
            p2Mode = 0;
        }
        //else the current p2Mode is less than or equal to the number of attack types,...
        else
        {
            //increment attack type
            p2Mode++;
        }
        // Debug.Log(p2Mode);
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
        #region Move Syncronization and Ammo Recharge Logic

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
                        //set timeBetweenGridSteps to double the value from inspector so that the charcater must wait outOfSyncMovementPenalty steps
                        timeBetweenGridSteps = inspectorTimeBetweenGridSteps * (1 / outOfSyncMovementPenalty);

                        //wait outOfSyncMovementPenalty steps
                        canStep = false;
                        tempTimeBetweenGridSteps = 0f;
                    }
                }

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
                    canStep = false;
                    tempTimeBetweenGridSteps = 0f;

                    //change direction of character
                    characterCenter.transform.localEulerAngles = moveVec3;
                }

                #endregion
            }
        }

        #endregion

        //***** Handle Ammo Recharge *****
        #region Ammo Recharge

        //if p1 is moving,...
        if(p1MoveDir != Vector2.zero)
        {
            //increment tempP1AmmoRechargeTime
            tempP1AmmoRechargeTime += Time.fixedDeltaTime;

            //if p1's ammo is less than maxAmmo,...
            if (p1Ammo < maxAmmo)
            {
                //if ammoRechargeTime has elapsed,...
                if (tempP1AmmoRechargeTime >= ammoRechargeTime)
                {
                    //increment p1Ammo by 1 keeping
                    p1Ammo++;

                    //reset tempP1AmmoRechargeTime
                    tempP1AmmoRechargeTime = 0f;
                }
            }
        }

        //if p2 is moving,...
        if(p2MoveDir != Vector2.zero)
        {
            //increment tempP2AmmoRechargeTime
            tempP2AmmoRechargeTime += Time.fixedDeltaTime;

            //if p2's ammo is less than maxAmmo,...
            if (p2Ammo < maxAmmo)
            {
                //if ammoRechargeTime has elapsed,...
                if (tempP2AmmoRechargeTime >= ammoRechargeTime)
                {
                    //increment p2Ammo by 1 keeping
                    p2Ammo++;

                    //reset tempP2AmmoRechargeTime
                    tempP2AmmoRechargeTime = 0f;
                }
            }
        }

        #endregion

        //***** Handle character attacking ******
        #region Attack Logic

        //if both players inputting an attack in the same direction,...
        if (p1AttackDir == p2AttackDir)
        {
            //if both players are attacking and syncedFireRate has elapsed and both players have ammo,...
            if (p1AttackDir != Vector2.zero && p2AttackDir != Vector2.zero && tempSyncedFireRate >= fireRate && p1Ammo > 0 && p2Ammo > 0)
            {
                //create a syncedProjectile at p1Center in orientation of p1Center (since both players are attacking in the same direction)
                Instantiate(syncedProjectile, p1Center.transform.position, p1Center.transform.rotation);

                //reset tempSyncedFireRate
                tempSyncedFireRate = 0f;

                //decrement p1Ammmo and p2Ammo
                p1Ammo--;
                p2Ammo--;
            }
        }
        else
        {
            //is p1 is inputting an attack direction and p1 can attack again and p1 has ammo,...
            if (p1AttackDir != Vector2.zero && tempP1FireRate >= fireRate && p1Ammo > 0)
            {
                //create a p1Projectile at p1Center in orientation of p1Center
                Instantiate(p1Projectile, p1Center.transform.position, p1Center.transform.rotation);

                //reset tempP1FireRate
                tempP1FireRate = 0f;

                //decrement p1Ammmo
                p1Ammo--;
            }

            //is p2 is inputting an attack direction and p2 can attack again and p2 has ammo,...
            if (p2AttackDir != Vector2.zero && tempP2FireRate >= fireRate && p2Ammo > 0)
            {
                //create a p2Projectile at p2Center in orientation of p2Center
                Instantiate(p2Projectile, p2Center.transform.position, p2Center.transform.rotation);

                //reset tempP2FireRate
                tempP2FireRate = 0f;

                //decrement p2Ammmo
                p2Ammo--;
            }
        }

        //increment attack timers
        tempP1FireRate += Time.fixedDeltaTime;
        tempP2FireRate += Time.fixedDeltaTime;
        tempSyncedFireRate += Time.fixedDeltaTime;

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

    public enum PlayerMode
    { 
        MOVE, ATTACK
    }
}