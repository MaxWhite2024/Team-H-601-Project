using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FFPlayerMovement : MonoBehaviour
{
    //number vars
    [SerializeField] private float moveSpeed;
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
        p1MoveDir = value.Get<Vector2>();
    }

    //when p2 pressed Arrow Keys,...
    void OnP2Move(InputValue value)
    {
        //set p2MoveDir to direction of Arrow Keys
        //NOTE: vector is already normalized!
        p2MoveDir = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        //calculate characterMoveDir
        characterMoveDir = p1MoveDir + p2MoveDir;

        //apply characterMoveDir to player
        rb.AddForce(characterMoveDir * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);

        //change direction of character
        transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, characterMoveDir));
    }
}
