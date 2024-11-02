using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement Vars")]
    [SerializeField] private float speed;
    [SerializeField] private float stepTime; //Time between steps

    [Header("Player + EnemyRB")]
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D rb;
    
    [HideInInspector]
    public EnemySpawner spawner;
    [HideInInspector]
    public RoomManager room;

    //public bool spawned = true;
    private Vector3 playerPos;
    private Vector3 distance;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {

        //Gets player, room, and rb if not assigned through inspector
        if(!player)
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

        playerPos = player.transform.position;
        timer = stepTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Counts down from stepTime, once time hits 0 moves in the direction of the player
        timer -= Time.deltaTime;
        if (timer <= 0) {

            playerPos = player.transform.position;

            distance = (playerPos - transform.position);
            rb.velocity = (distance.normalized * speed);
            //transform.position += distance.normalized * speed;

            timer = stepTime;
        }
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
