using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private float spawnTimer;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = spawnTimer;    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            timer = spawnTimer;
            Instantiate(enemy, transform.position, transform.rotation); //Spawns the projectile in the player
        }
    }
}
