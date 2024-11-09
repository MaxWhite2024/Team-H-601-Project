using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    [Header("Spawn Settings")]
    [SerializeField] public float spawnTimer;
    [SerializeField] private GameObject enemy;
    [SerializeField] private bool offWhenRoomClean;
    public bool decreaseSpawnTime;
    [SerializeField] private int maxEnemies;

    [Header("Debug Vars")]
    [SerializeField] private RoomManager room;
    public int enemiesSpawned;
    [SerializeField] private float timer;


    // Start is called before the first frame update
    void Start()
    {
        timer = spawnTimer;
        enemiesSpawned = 0;

        if(room == null)
        {
            room = transform.parent.gameObject.GetComponent<RoomManager>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Stops spawning if the room is clean and offWhenRoomClean is true
        if (room.roomClean && offWhenRoomClean)
        {
            this.GetComponent<EnemySpawner>().enabled = false;
        }

        //Counts down from spawnTimer, when it reaches 0 it checks to see if there are too many enemies
        //If there aren't too many enemies, it will spawn a new one and add them to lists in the room
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            timer = spawnTimer;

            if(enemiesSpawned < maxEnemies && room.enemies.Count < room.maxEnemies)
            {
                Enemy newEnemy = Object.Instantiate(enemy, transform.position, transform.rotation, transform.parent).GetComponent<Enemy>(); //Spawns the enemy as a child of the roomManager
                newEnemy.spawner = this;
                newEnemy.room = room;
                room.enemies.Add(newEnemy);
                room.damageables.Add(newEnemy.gameObject.GetComponent<Damageable>());
                enemiesSpawned++;
            }
        }
    }

    /// <summary>
    /// Removes the spawner from the room and then deletes itself
    /// </summary>
    public void Death()
    {

        if (room != null)
        {
            room.spawners.Remove(this);
        }

        Destroy(this.gameObject);
    }

}
