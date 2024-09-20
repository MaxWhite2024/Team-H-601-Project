using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FFPlayerMovement : MonoBehaviour
{
    //number vars
    [SerializeField] private float moveSpeed, timeBetweenGridSteps;
    [SerializeField] private bool isGridMovement = false, canStep = true;
    private Vector2 p1MoveDir, p2MoveDir, characterMoveDir;
    
    //componment vars
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //get rgidbody2D of player
        rb = GetComponent<Rigidbody2D>();

        //set players' move directions to zero
        p1MoveDir = Vector2.zero;
        p2MoveDir = Vector2.zero;
        characterMoveDir = Vector2.zero;
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

    void FixedUpdate()
    {
        if(isGridMovement)
        {
            //calculate grid characterMoveDir
            if(p1MoveDir == p2MoveDir)
            {
                characterMoveDir = p1MoveDir * 2f;
            }
            else
            {
                characterMoveDir = Vector2.zero;
            }

            //if character can step,...
            if(canStep)
            {
                //apply characterMoveDir to player
                rb.AddForce(characterMoveDir * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);

                //begin step cooldown
                StartCoroutine(GridStepDelay(timeBetweenGridSteps));
            }
        }
        else
        {
            //calculate freeform characterMoveDir
            characterMoveDir = p1MoveDir + p2MoveDir;

            //apply characterMoveDir to player
            rb.AddForce(characterMoveDir * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        //if players have made new movement inputs,...
        if(characterMoveDir != Vector2.zero)
        {
            //change direction of character
            transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, characterMoveDir));
        }
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