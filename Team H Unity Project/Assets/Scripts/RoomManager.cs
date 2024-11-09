using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    
    [Header("Doors and order")] public List<Door> doors;
    [SerializeField] private bool firstRoom;
    

    [Header("Customization Vars")]
    public Transform cameraTransform;
    public int maxEnemies;
    [SerializeField] private bool fullClear; //Do you need to kill all enemies or all everything

    [Header("Spawners Speed Up Vars")]
    [SerializeField] private float decreaseSpawnRateRate;
    [SerializeField] private float decreaseSpawnRateAmount;
    [SerializeField] private float decreaseSpawnRateMinimum;
    private float timeInRoom;

    [Header("Debugging Vars")]
    public List<Enemy> enemies;
    public List<EnemySpawner> spawners;
    public List<Damageable> damageables;
    public bool roomClean;

    // Start is called before the first frame update
    private void Start()
    {
        roomClean = false;

        //Goes through each child of the room and adds them to the appropriate list
        foreach(Transform child in transform)
        {
            Enemy enemy = child.gameObject.GetComponent<Enemy>();
            EnemySpawner spawner = child.gameObject.GetComponent<EnemySpawner>();
            Damageable damageable = child.gameObject.GetComponent<Damageable>();
            if (enemy != null && !enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
            else if(spawner != null && !spawners.Contains(spawner))
            {
                spawners.Add(spawner);
            }
            
            
            if (damageable != null && !damageables.Contains(damageable))
            {
                damageables.Add(damageable);
            }
        }

        //Disables self if not the first room
        if(!firstRoom)
        {
            this.gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        //When the room is enabled, if it's not clean and the door isn't moving the camera, disable the door
        if(!roomClean)
        {
            foreach (Door door in doors)
            {
                if (!door.changingRooms)
                {
                    door.gameObject.SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        //Activate each door when room is clean
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

        //If the enemies list is empty and the room doesn't need to be full cleared, OR if the damageables list is empty
        switch(fullClear)
        {
            case true:
                if (damageables.Count <= 0)
                {
                    roomClean = true;
                }
                break;
            default:
                if (enemies.Count <= 0)
                {
                    roomClean = true;
                }
                break;
        }

        //Counts up to decreaseSpawnRateRate, once timeInRoom reachest that if goes through each spawner to see if they spawn faster over time
        //If they do spawn faster over time, their rate is decreased by decreaseSpawnRateAmount as long as the spawnrate is > decreaseSpawnRateMinimum
        timeInRoom += Time.deltaTime;
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
