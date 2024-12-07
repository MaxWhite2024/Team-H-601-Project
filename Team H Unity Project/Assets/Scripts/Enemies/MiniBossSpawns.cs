using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class MiniBossSpawns : Enemy
{

    private bool targetingPlayer = false;

    [Header("Initial movement values")]
    [SerializeField] private Vector2 horizontal;
    [SerializeField] private Vector2 vertical;

    [Header("Boss Spawn specific debug")]
    [SerializeField] private Vector3 nextTarget;

    // Start is called before the first frame update
    void Start()
    {
        base.DeclareVars();
        SetPath();
        if (base.sprite == null)
        {
            base.sprite = transform.GetChild(1).gameObject;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (targetingPlayer)
        {
            base.UpdatePlayerPos();
            path.destination = playerPos;
            if (sprite != null)
            {
                Vector3 move = (playerPos - sprite.transform.position);
                sprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (Mathf.Rad2Deg * Mathf.Atan2(-move.x, move.y)) + 180));
            }
            return;
        }

        if(ReachTarget())
        {
            targetingPlayer = true;
        }
        else
        {
            if (sprite != null)
            {
                Vector3 move = (nextTarget - sprite.transform.position);
                sprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (Mathf.Rad2Deg * Mathf.Atan2(-move.x, move.y)) + 180));
            }
        }
        
    }

    private bool ReachTarget()
    {

        if (transform.position.x <= nextTarget.x + .1 && transform.position.x >= nextTarget.x - .1) //If the x is within .1 of the target x
        {
            if (transform.position.y <= nextTarget.y + .1 && transform.position.y >= nextTarget.y - .1) //If the y is within .1 of the target y
            {
                return true;
            }
        }

        return false;
    }

    public void SetPath()
    {
        nextTarget = new Vector3(0, 0, 0);
        nextTarget.x = Random.Range(horizontal.x, horizontal.y);
        nextTarget.y = Random.Range(vertical.x, vertical.y);

        path.destination = nextTarget;
    }
}
