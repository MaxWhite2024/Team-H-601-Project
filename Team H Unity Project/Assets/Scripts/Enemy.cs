using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private float speed, stepTime;
    [SerializeField] private Rigidbody2D rb;


    //public bool spawned = true;
    private Vector3 playerPos;
    private Vector3 distance;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        if(!player)
        {
            player = GameObject.Find("Players");
        }
        playerPos = player.transform.position;
        timer = stepTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        timer -= Time.deltaTime;
        if (timer <= 0) {

            playerPos = player.transform.position;

            distance = (playerPos - transform.position);
            rb.velocity = (distance.normalized * speed);
            //transform.position += distance.normalized * speed;

            timer = stepTime;
        }
    }
}
