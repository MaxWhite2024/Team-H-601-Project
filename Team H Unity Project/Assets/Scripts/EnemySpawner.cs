using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] public float spawnTimer;
    [SerializeField] private RoomManager room;
    
    //Spawn Settings
    [SerializeField] private bool offWhenRoomClean;
    public bool decreaseSpawnTime;

    private float timer;
    public int enemiesSpawned;
    [SerializeField] private int maxEnemies;

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
        if (room.roomClean && offWhenRoomClean)
        {
            this.GetComponent<EnemySpawner>().enabled = false;
        }

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

    public void Death()
    {

        if (room != null)
        {
            room.spawners.Remove(this);
        }

        Destroy(this.gameObject);
    }

}
