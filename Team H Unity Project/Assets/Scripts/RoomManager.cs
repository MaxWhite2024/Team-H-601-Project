using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public bool roomClean;
    public List<Enemy> enemies;
    public List<EnemySpawner> spawners;

    public int maxEnemies;
    private float timeInRoom;
    [SerializeField] private float decreaseSpawnRateRate;
    [SerializeField] private float decreaseSpawnRateAmount;
    [SerializeField] private float decreaseSpawnRateMinimum;

    // Start is called before the first frame update
    void Start()
    {
        roomClean = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeInRoom += Time.deltaTime;

        if(enemies.Count <= 0)
        {
            roomClean = true;
        }

        if (timeInRoom > decreaseSpawnRateRate)
        {

            foreach (EnemySpawner spawner in spawners)
            {
                if(spawner.decreaseSpawnTime && spawner.spawnTimer > decreaseSpawnRateMinimum)
                {
                    spawner.spawnTimer -= decreaseSpawnRateAmount;

                    if(spawner.spawnTimer < decreaseSpawnRateMinimum)
                    {
                        spawner.spawnTimer = decreaseSpawnRateMinimum;
                    }
                }
            }

            timeInRoom -= decreaseSpawnRateRate;
        }
    }
}
