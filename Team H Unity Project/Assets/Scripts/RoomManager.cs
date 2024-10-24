using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public bool roomClean;
    public List<Enemy> enemies;
    public List<EnemySpawner> spawners;
    public List<Door> doors;
    public Transform cameraTransform;

    public int maxEnemies;
    private float timeInRoom;
    [SerializeField] private float decreaseSpawnRateRate;
    [SerializeField] private float decreaseSpawnRateAmount;
    [SerializeField] private float decreaseSpawnRateMinimum;

    // Start is called before the first frame update
    private void Start()
    {
        roomClean = false;

        foreach(Transform child in transform)
        {
            Enemy enemy = child.gameObject.GetComponent<Enemy>();
            EnemySpawner spawner = child.gameObject.GetComponent<EnemySpawner>();
            if (enemy != null && !enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
            else if(spawner != null && !spawners.Contains(spawner))
            {
                spawners.Add(spawner);
            }
        }
    }

    void OnEnable()
    {
        if(!roomClean)
        {
            foreach (Door door in doors)
            {
                if (!door.movingCamera)
                {
                    door.gameObject.SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(roomClean)
        {
            foreach (Door door in doors)
            {
                if(!door.gameObject.activeSelf)
                {
                    door.gameObject.SetActive(true);
                }
            }
        }
        
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
