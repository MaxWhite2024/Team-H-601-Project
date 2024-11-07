using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Damageable : MonoBehaviour
{
    enum Damageables {Unassigned, Enemy, Player, Trash, Spawner};

    public int maxHealth;
    public int health;
    [SerializeField] private float iFrameTime;
    [SerializeField] private Damageables type;
    
    private float timer;
    [HideInInspector] public RoomManager room;

    // Start is called before the first frame update
    void Start()
    {
        timer = iFrameTime;
        if (room == null && transform.parent != null)
        {
            room = transform.parent.gameObject.GetComponent<RoomManager>();
        }

        if(type == Damageables.Unassigned)
        {
            if (this.gameObject.GetComponent<PlayerMovementAndAttack>() != null)
            {
                type = Damageables.Player;
            }
            //Custom enemy and spawner methods
            else if (this.gameObject.GetComponent<Enemy>() != null)
            {
                type = Damageables.Enemy;
            }
            else if (this.gameObject.GetComponent<EnemySpawner>() != null)
            {
                type = Damageables.Spawner;
            }
            else
            {
                type = Damageables.Trash;
            }
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

    /// <summary>
    /// Both trigger collidions and collision collisions are handled the same way. To remove repeat code this method handles all collision logic
    /// </summary>
    /// <param name="collider">The collider hitting this gameObject</param>
    private void CollisionLogic(Collider2D collider)
    {
        //This is so enemies won't damage each other or damage trash
        if (collider.gameObject.layer == this.gameObject.layer)
        {
            return;
        }

        DamageSource damage = collider.gameObject.GetComponent<DamageSource>();

        //If there is damage coming in AND the iFrames ran out
        if (damage != null && timer <= 0)
        {
            //Sets iFrames, takes damage, and takes knockback based on damage taken
            timer = iFrameTime;
            health = health - damage.damage;
            Vector3 knockbackAngle = -1 * (collider.gameObject.transform.position - this.gameObject.transform.position);
            this.gameObject.GetComponent<Rigidbody2D>().velocity = (knockbackAngle.normalized * damage.damage * 4);

            //Death methods
            if (health <= 0)
            {
                //Removes self from room
                if (room != null)
                {
                    room.damageables.Remove(this);
                }

                switch (type)
                {
                    case Damageables.Player:
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        break;
                    case Damageables.Enemy:
                        this.gameObject.GetComponent<Enemy>().Death();
                        break;
                    case Damageables.Spawner:
                        this.gameObject.GetComponent<EnemySpawner>().Death();
                        break;
                    default:
                        AstarPath.active.Scan(); //Reloads graph
                        Destroy(this.gameObject);
                        break;
                }
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        CollisionLogic(collider.collider);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        CollisionLogic(collider);
    }
}
