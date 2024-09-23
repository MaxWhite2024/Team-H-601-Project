using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private float speed, stepTime;

    private Vector3 playerPos;
    private Vector3 distance;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
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
            transform.position += distance.normalized * speed;

            timer = stepTime;
        }
    }
}
