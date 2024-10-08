using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private float spawnTimer;
    [SerializeField] private RoomManager room;

    private float timer;
    public int enemiesSpawned;

    // Start is called before the first frame update
    void Start()
    {
        timer = spawnTimer;
        enemiesSpawned = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            timer = spawnTimer;
            Enemy newEnemy = Object.Instantiate(enemy, transform.position, transform.rotation).GetComponent<Enemy>(); //Spawns the projectile in the player
            newEnemy.spawner = this;
            enemiesSpawned++;
        }
    }
}
