using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    [Header("Doors and order")] public List<Door> doors;
    [SerializeField] private bool firstRoom;
    public Transform cameraTransform;

    [Header("Customization Vars")]
    public int maxEnemies;
    [SerializeField] private bool fullClear; //Do you need to kill all enemies or all everything
    public float healthDropChance = -1;
    [SerializeField] private bool cleanTrashOnClear = true;

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
    [SerializeField] private HouseManager house;
    private float cleanTimer = 10;

    [Header("Particle System Vars")]
    [SerializeField] private new ParticleSystem particleSystem;

    // Start is called before the first frame update
    private void Start()
    {

        roomClean = false;

        //Goes through each child of the room and adds them to the appropriate list
        foreach (Transform child in transform)
        {
            Enemy enemy = child.gameObject.GetComponent<Enemy>();
            EnemySpawner spawner = child.gameObject.GetComponent<EnemySpawner>();
            Damageable damageable = child.gameObject.GetComponent<Damageable>();
            if (enemy != null && !enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
            else if (spawner != null && !spawners.Contains(spawner))
            {
                spawners.Add(spawner);
            }


            if (damageable != null && !damageables.Contains(damageable))
            {
                damageables.Add(damageable);
            }
        }

        if (house == null)
        {
            house = transform.parent.gameObject.GetComponent<HouseManager>();
        }

        if (healthDropChance < 0)
        {
            healthDropChance = house.healthDropChance;
        }

        //Disables self if not the first room
        if (!firstRoom)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            house.activeRoom = this;
        }
    }

    void OnEnable()
    {
        //When the room is enabled, if it's not clean and the door isn't moving the camera, disable the door
        if (!roomClean)
        {
            foreach (Door door in doors)
            {
                if (!door.changingRooms)
                {
                    door.Close();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        //Activate each door when room is clean
        if (roomClean)
        {
            return;
        }

        RoomCleanCheck();

        //Counts up to decreaseSpawnRateRate, once timeInRoom reachest that if goes through each spawner to see if they spawn faster over time
        //If they do spawn faster over time, their rate is decreased by decreaseSpawnRateAmount as long as the spawnrate is > decreaseSpawnRateMinimum
        timeInRoom += Time.deltaTime;
        cleanTimer -= Time.deltaTime;
        if (timeInRoom > decreaseSpawnRateRate)
        {
            foreach (EnemySpawner spawner in spawners)
            {
                if (spawner.decreaseSpawnTime && spawner.spawnTimer > decreaseSpawnRateMinimum)
                {
                    spawner.spawnTimer -= decreaseSpawnRateAmount;

                    if (spawner.spawnTimer < decreaseSpawnRateMinimum)
                    {
                        spawner.spawnTimer = decreaseSpawnRateMinimum;
                    }
                }
            }

            timeInRoom -= decreaseSpawnRateRate;
        }

        if(cleanTimer <= 0)
        {
            PurgeNulls();
            cleanTimer = 2;
        }
    }

    public void PurgeNulls(bool full = false)
    {
        if (fullClear || full)
        {
            for(int i = 0; i < damageables.Count; i++)
            {
                if (damageables[i] == null)
                {
                    damageables.Remove(damageables[i]);
                    i--;
                }
            }

            return;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.Remove(enemies[i]);
                i--;
            }
        }
    }

    public bool RoomCleanCheck(bool force = false)
    {
        //Forcing room clean?
        roomClean = force;

        //If the enemies list is empty and the room doesn't need to be full cleared, OR if the damageables list is empty
        switch (fullClear)
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

        if(roomClean)
        {
            foreach (Door door in doors)
            {
                if (!door.open)
                {
                    door.Open();
                }
            }

            if (!particleSystem.isPlaying && !firstRoom)
            {
                //play sparkle particle system
                particleSystem.Play();
            }

            if (!fullClear && cleanTrashOnClear)
            {
                PurgeNulls(true);
                while (damageables.Count > 0)
                {
                    damageables[0].Death();
                }
            }

            house.RoomsClean();
        }

        return roomClean;
    }

    //DEBUG METHODS

    /// <summary>
    /// Destroys every damagable in the room
    /// </summary>
    public void DebugClear()
    {
        while (damageables.Count > 0)
        {
            if (damageables[0] == null)
            {
                damageables.Remove(damageables[0]);
            }
            else
            {
                damageables[0].Death();
            }
        }
    }

    /// <summary>
    /// Forces the room to be marked as clean
    /// </summary>
    public void DebugCleanRoom()
    {
        RoomCleanCheck(true);
    }
}
