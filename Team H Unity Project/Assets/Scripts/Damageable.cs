using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Damageable : MonoBehaviour
{
    enum Damageables {Unassigned, Enemy, Player, Trash, Spawner, MiniBoss};

    public int maxHealth;
    public int armor;
    public int health;
    [SerializeField] private float iFrameTime;
    [SerializeField] private Damageables type;

    [Header("Health Pickups")]
    [SerializeField] private bool dropHealth = false;
    [SerializeField] private GameObject healthPickup;
    
    private float timer;
    private float startScale;
    [HideInInspector] public RoomManager room;

    [Header("Player and Miniboss variables")]
    [SerializeField] private PlayerSpriteManager playerSpriteManager;
    [SerializeField] private BossSpriteManager bossSpriteManager;

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale.x;
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
        //if invulnerability timer has NOT yet elapsed,...
        if (timer > 0)
        {
            //decrement timer
            timer -= Time.fixedDeltaTime;
        }
        //else invulnerability timer has elapsed AND gameobject is the player,...
        else if (type == Damageables.Player)
        {
            //end player damage VFX
            playerSpriteManager.EndPlayerInvulFlashVFX();
        }
        //else invulnerability timer has elapsed AND gameobject is a mini-boss,...
        else if (type == Damageables.MiniBoss)
        {
            //end boss damage VFX
            bossSpriteManager.EndInvulFlashVFX();
        }
    }

    /// <summary>
    /// Both trigger collidions and collision collisions are handled the same way. To remove repeat code this method handles all collision logic
    /// </summary>
    /// <param name="collider">The collider hitting this gameObject</param>
    private void CollisionLogic(Collider2D collider)
    {
        //This is so enemies won't damage each other or damage trash
        if (collider.gameObject.layer == this.gameObject.layer || (collider.gameObject.layer == 6 && type == Damageables.Trash))
        {
            return;
        }

        DamageSource damage = collider.gameObject.GetComponent<DamageSource>();

        if(damage == null)
        {
            return;
        }

        if (damage.healing)
        {
            //If it is a healing item and this is not a player, nothign should happen
            if (type != Damageables.Player)
            {
                return;
            }
            health += damage.damage;

            if(health > maxHealth)
            {
                health = maxHealth;
            }

            Destroy(damage.gameObject);
            return;
        }

        //If there is damage coming in AND the iFrames ran out
        if (timer <= 0)
        {
            if(armor >= damage.damage)
            {
                return;
            }
            //Anything after this point knows damage is more than armor (also non-zero)

            //Sets iFrames, takes damage, and takes knockback based on damage taken
            timer = iFrameTime;
            health = health + armor - damage.damage;

            //if gameobject is the player,...
            if(type == Damageables.Player)
            {
                //start player damage VFX
                playerSpriteManager.StartPlayerDamageVFX();
            }
            //else if gameobject is a miniboss,...
            else if(type == Damageables.MiniBoss)
            {
                //start boss damage VFX
                bossSpriteManager.StartInvulFlashVFX();
            }
            

            //Death methods
            if (health <= 0)
            {
                Death();
                return;
            }

            Vector3 knockbackAngle;
            switch (type)
            {
                case Damageables.Enemy:
                    if(!this.gameObject.GetComponent<Enemy>().canMove) //No knockback if enemy can't move
                    {
                        return;
                    }
                    knockbackAngle = -1 * (collider.gameObject.transform.position - this.gameObject.transform.position);
                    this.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackAngle.normalized * (armor-damage.damage) * 1000 * -1);
                    break;
                case Damageables.Player:
                    knockbackAngle = -1 * (collider.gameObject.transform.position - this.gameObject.transform.position);
                    this.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackAngle.normalized * (armor-damage.damage) * 500 * -1);
                    break;
                case Damageables.MiniBoss:
                    float scale = startScale * (((float)health / ((float)maxHealth * 2)) + .5f);
                    transform.localScale = new Vector3(scale, scale, 1);
                    this.gameObject.GetComponent<MiniBoss>().UpdateVars((float)health / (float)maxHealth);
                    break;
                default:
                    break;
            }
        }
    }

    private void CollisionStayCheck(Collider2D collider)
    {
        if(type != Damageables.Player) //Enemies should not be able to take damage from the same projectile
        {
            return;
        }
        if(timer > 0)
        {
            return;
        }

        CollisionLogic(collider);
    }

    private void Death()
    {
        //reset Time.timeScale
        Time.timeScale = 1f;

        //Removes self from room
        if (room != null)
        {
            room.damageables.Remove(this);

            if(dropHealth)
            {
                if(Random.Range(0, 100) <= room.healthDropChance)
                {
                    Instantiate(healthPickup, transform.position, transform.rotation, transform.parent);
                }
            }
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
            case Damageables.Trash:
                //Recalculates the Astar grid at the position of this object
                AstarPath.active.UpdateGraphs(gameObject.GetComponent<Collider2D>().bounds);
                Destroy(this.gameObject);
                break;
            default:
                Destroy(this.gameObject);
                break;
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

    private void OnCollisionStay2D(Collision2D collider)
    {
        CollisionStayCheck(collider.collider);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        CollisionStayCheck(collider);
    }
}
