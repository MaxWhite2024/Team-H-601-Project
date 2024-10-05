using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_arrowscript : MonoBehaviour
{
    [SerializeField]private RectTransform P1_Arrow;
    [SerializeField]private RectTransform P2_Arrow;
    [SerializeField]private FFPlayerMovement PlayerMovementScript;
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovementScript = GameObject.Find("Players").GetComponent<FFPlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovementScript.p1MoveDir == Vector2.up) 
        {
            P1_Arrow.rotation = Quaternion.Euler(0f, 0f, 0f);
        
        }
        else if(PlayerMovementScript.p1MoveDir == Vector2.left)
        {
            P1_Arrow.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else if (PlayerMovementScript.p1MoveDir == Vector2.right)
        {
            P1_Arrow.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else if (PlayerMovementScript.p1MoveDir == Vector2.down)
        {
            P1_Arrow.rotation = Quaternion.Euler(0f, 0f, 180f);
        }

        if (PlayerMovementScript.p2MoveDir == Vector2.up)
        {
            P2_Arrow.rotation = Quaternion.Euler(0f, 0f, 0f);

        }
        else if (PlayerMovementScript.p2MoveDir == Vector2.left)
        {
            P2_Arrow.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else if (PlayerMovementScript.p2MoveDir == Vector2.right)
        {
            P2_Arrow.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else if (PlayerMovementScript.p2MoveDir == Vector2.down)
        {
            P2_Arrow.rotation = Quaternion.Euler(0f, 0f, 180f);
        }

    }
}
