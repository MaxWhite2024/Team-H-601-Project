using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Projectile Stuff")]
    [SerializeField] private GameObject projectile;
    [SerializeField] public float attackTimer;

    [Header("Projectile Debug Vars")]
    [SerializeField] private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = attackTimer;

        base.DeclareVars();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.UpdatePlayerPos();

        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            Shoot();
            timer = attackTimer;
        }
    }

    void Shoot()
    {
        float offset = -90;
        Vector2 direction = playerPos - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Instantiate(projectile, transform.position, Quaternion.Euler(Vector3.forward * (angle + offset)));
    }
}
