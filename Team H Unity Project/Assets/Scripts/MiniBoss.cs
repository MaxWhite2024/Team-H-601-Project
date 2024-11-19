using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float startSpawnTimer;
    [SerializeField] private float minSpawnTimer;
    [SerializeField] private GameObject enemy;

    [Header("Debug Vars")]
    [SerializeField] private List<Transform> spawnLocations;
    [SerializeField] private RoomManager room;
    [SerializeField] private float currentSpawnTimer;
    private float spawnWiggleRoom;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = (startSpawnTimer/10);
        spawnWiggleRoom = startSpawnTimer - minSpawnTimer;
        currentSpawnTimer = startSpawnTimer;

        if (room == null)
        {
            room = transform.parent.gameObject.GetComponent<RoomManager>();
        }
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
    }

    public void UpdateSpawnTime(float healthPercent)
    {
        currentSpawnTimer = (spawnWiggleRoom * healthPercent) + minSpawnTimer;


    }
}
