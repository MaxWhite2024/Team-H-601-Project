using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Damageable : MonoBehaviour
{
    public int health;
    [SerializeField] private float iFrameTime;
    private float timer;
    public RoomManager room;

    // Start is called before the first frame update
    void Start()
    {
        timer = iFrameTime;
        if (room == null && transform.parent != null)
        {
            room = transform.parent.gameObject.GetComponent<RoomManager>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }

    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        DamageSource damage = collider.gameObject.GetComponent<DamageSource>();
        if (damage != null && timer <= 0)
        {
            timer = iFrameTime;
            health = health - damage.damage;
            Vector3 knockbackAngle = -1 * (collider.gameObject.transform.position - this.gameObject.transform.position);
            this.gameObject.GetComponent<Rigidbody2D>().velocity = (knockbackAngle.normalized * damage.damage * 4);

            if (health <= 0)
            {

                if(room != null)
                {
                    room.damageables.Remove(this);
                }

                if (this.gameObject.GetComponent<PlayerMovementAndAttack>() != null)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else if(this.gameObject.GetComponent<Enemy>() != null)
                {
                    this.gameObject.GetComponent<Enemy>().Death();
                }
                else if (this.gameObject.GetComponent<EnemySpawner>() != null)
                {
                    this.gameObject.GetComponent<EnemySpawner>().Death();
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log(this.gameObject.name + " has been hit by " + collider.gameObject.name);
        //Debug.Log(collider.gameObject.transform.position - this.gameObject.transform.position);

        DamageSource damage = collider.gameObject.GetComponent<DamageSource>();

        //TEMP FIX TO AVOID PLAYER DAMAGING THEMSELVES
        if(this.gameObject.GetComponent<PlayerMovementAndAttack>() != null)
        {
            return;
        }

        if (damage != null && timer <= 0)
        {
            timer = iFrameTime;
            health = health - damage.damage;
            Vector3 knockbackAngle = -1 * (collider.gameObject.transform.position - this.gameObject.transform.position);
            this.gameObject.GetComponent<Rigidbody2D>().velocity = (knockbackAngle.normalized * damage.damage * 4);

            if (health <= 0)
            {
                if (room != null)
                {
                    room.damageables.Remove(this);
                }

                if (this.gameObject.GetComponent<Enemy>() != null)
                {
                    this.gameObject.GetComponent<Enemy>().Death();
                }
                else if (this.gameObject.GetComponent<EnemySpawner>() != null)
                {
                    this.gameObject.GetComponent<EnemySpawner>().Death();
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
            
        }
    }
}
