using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float deleteTimer = 5f;
    [SerializeField] private bool canPierceEnemies = false;
    private int enemyLayer;

    void Awake()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        //Debug.Log(enemyLayer);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.up * Time.fixedDeltaTime * moveSpeed;
        deleteTimer -= Time.fixedDeltaTime;

        if (deleteTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //destroy self
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.gameObject.layer);
        Debug.Log(canPierceEnemies && collider.gameObject.layer == enemyLayer);
        //if projectile can pierce enemies and touched rigidbody is an enemy,...
        if (canPierceEnemies && collider.gameObject.layer == enemyLayer)
        {
            //exit function
            return;
        }
        //else projectile CANNOT pierce enemies or touched rigidbody is NOT an enemy,...
        else
        {
            //destroy self
            Destroy(this.gameObject);
        }
    }
}
