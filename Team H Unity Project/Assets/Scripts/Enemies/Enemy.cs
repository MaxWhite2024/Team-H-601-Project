using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [Header("Movement Vars")]
    [SerializeField] protected float speed;
    public bool canMove;
    //[SerializeField] private float stepTime; //Time between steps, only used in old pathfinding

    [Header("Player + EnemyRB")]
    [SerializeField] protected GameObject player;
    [SerializeField] protected Rigidbody2D rb;

    [Header("Debug Vars")]
    public EnemySpawner spawner;
    public RoomManager room;
    [SerializeField] protected GameObject sprite;

    //public bool spawned = true;
    protected Vector3 playerPos;
    protected Vector3 distance;
    //protected float timer; //Only used in old pathfinding

    //Astar vars
    protected AIPath path;

    // Start is called before the first frame update
    void Start()
    {
        //This is so children can declare all the vars needed
        DeclareVars();
        if (sprite == null)
        {
            sprite = transform.GetChild(1).gameObject;
        }
    }

    protected void DeclareVars()
    {
        //Gets player, room, and rb if not assigned through inspector
        if (!player)
        {
            player = GameObject.Find("Players");
        }
        if (!rb)
        {
            rb = this.GetComponent<Rigidbody2D>();
        }
        if (room == null)
        {
            room = transform.parent.gameObject.GetComponent<RoomManager>();
        }

        if (canMove)
        {
            path = GetComponent<AIPath>();
            path.maxSpeed = speed;
        }

        //playerPos = player.transform.position; - technically a useless call??
        //timer = stepTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        UpdatePlayerPos();
        path.destination = playerPos;

        if(sprite != null)
        {
            Vector3 move = (playerPos - sprite.transform.position);
            sprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (Mathf.Rad2Deg * Mathf.Atan2(-move.x, move.y)) + 180));
        }
        


        #region old pathing
        /*//Counts down from stepTime, once time hits 0 moves in the direction of the player
        timer -= Time.deltaTime;
        if (timer <= 0) {

            playerPos = player.transform.position;

            distance = (playerPos - transform.position);
            rb.velocity = (distance.normalized * speed);
            //transform.position += distance.normalized * speed;

            timer = stepTime;
        }*/
        #endregion
    }

    protected void UpdatePlayerPos()
    {
        playerPos = player.transform.position;
    }

    /// <summary>
    /// Removes enemy from the spawner and room enemy lists, then destroys itself
    /// </summary>
    public void Death()
    {
        if (spawner != null)
        {
            spawner.enemiesSpawned--;
        }
        if(room != null)
        {
            room.enemies.Remove(this);
        }

        Destroy(this.gameObject);
    }

}
