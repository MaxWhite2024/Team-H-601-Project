using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public bool roomClean;
    public List<Enemy> enemies;
    public List<EnemySpawner> spawners;

    private float timeInRoom;

    // Start is called before the first frame update
    void Start()
    {
        roomClean = false;
        enemies = new List<Enemy>();
        spawners = new List<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
