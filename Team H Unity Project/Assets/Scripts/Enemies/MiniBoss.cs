using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MiniBoss : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float startSpawnTimer;
    [SerializeField] private float minSpawnTimer;
    [SerializeField] private GameObject enemy;

    [Header("Move Settings")]
    [SerializeField] private Vector2 horizontalMinMax;
    [SerializeField] private Vector2 verticalMinMax;
    [SerializeField] private float speedMin;
    [SerializeField] private float speedMax;


    [Header("Debug Vars")]
    [SerializeField] private List<Transform> spawnLocations;
    [SerializeField] private RoomManager room;
    [SerializeField] private float currentSpawnTimer;
    [SerializeField] private float currentSpeed;
    [SerializeField] private Vector3 nextTarget;

    private float spawnWiggleRoom;
    private float speedWiggleRoom;
    private float timer;
    private AIPath path;
    // Start is called before the first frame update
    void Start()
    {
        timer = (startSpawnTimer/10);
        spawnWiggleRoom = startSpawnTimer - minSpawnTimer;
        currentSpawnTimer = startSpawnTimer;

        speedWiggleRoom = speedMax - minSpawnTimer;
        currentSpeed = speedMin;

        if (room == null)
        {
            room = transform.parent.gameObject.GetComponent<RoomManager>();
        }

        path = GetComponent<AIPath>();
        path.maxSpeed = currentSpeed;
        SetPath();

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            timer = currentSpawnTimer;

            foreach(Transform spawnTransform in spawnLocations)
            {
                Enemy newEnemy = Object.Instantiate(enemy, spawnTransform.position, spawnTransform.rotation, transform.parent).GetComponent<Enemy>(); //Spawns the enemy as a child of the roomManager
                newEnemy.room = room;
                room.enemies.Add(newEnemy);
                room.damageables.Add(newEnemy.gameObject.GetComponent<Damageable>());
            }
        }

        if(ReachTarget())
        {
            SetPath();
        }
    }

    private bool ReachTarget()
    {

        if(transform.position.x <= nextTarget.x +.1 && transform.position.x >= nextTarget.x - .1) //If the x is within .1 of the target x
        {
            if (transform.position.y <= nextTarget.y + .1 && transform.position.y >= nextTarget.y - .1) //If the y is within .1 of the target y
            {
                return true;
            }
        }

        return false;
    }

    public void UpdateVars(float healthPercent)
    {
        //Debug.Log(healthPercent);
        currentSpawnTimer = (spawnWiggleRoom * healthPercent) + minSpawnTimer;
        if (timer > currentSpawnTimer)
        {
            timer = currentSpawnTimer;
        }
        currentSpeed = (speedWiggleRoom * (1-healthPercent)) + speedMin;
        path.maxSpeed = currentSpeed;
        //Debug.Log(currentSpeed);
    }

    public void SetPath()
    {
        nextTarget = new Vector3(0, 0, 0);
        nextTarget.x = Random.Range(horizontalMinMax.x, horizontalMinMax.y);
        nextTarget.y = Random.Range(verticalMinMax.x, verticalMinMax.y);

        path.destination = nextTarget;
    }
}
