using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFPlayerMovement : MonoBehaviour
{
    //number vars
    [SerializeField] private float moveSpeed;
    private float p1moveX, p1moveY, p2moveX, p2moveY;
    private Vector2 p1moveDir, p2moveDir;
    
    //componment vars
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //get rgidbody2D of player
        rb = GetComponent<Rigidbody2D>();

        //set players' move directions to zero
        p1moveDir = Vector2.zero;
        p2moveDir = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //get p1's movement direction from keyboard input
        if(Input.GetKey("w"))
        {
            p1moveDir = Vector2.up;
        }
        if(Input.GetKey("d"))
        {
            p1moveDir = Vector2.right;
        }
        if(Input.GetKey("s"))
        {
            p1moveDir = Vector2.down;
        }
        if(Input.GetKey("a"))
        {
            p1moveDir = Vector2.left;
        }

        Debug.DrawRay(transform.position, p1moveDir.normalized * 100f, Color.red);
    }

    void FixedUpdate()
    {
        //apply p1moveDir and p2moveDir to player
        rb.AddForce((p1moveDir.normalized + p2moveDir.normalized) * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
    }
}
