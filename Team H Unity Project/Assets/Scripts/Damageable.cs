using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("I've been hit by " + collision.gameObject.name);
        DamageSource damage = collision.gameObject.GetComponent<DamageSource>();
        if (damage != null)
        {
            health = health - damage.damage;
            if(health <= 0)
            {
                Destroy(this.gameObject);
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("I've been hit by " + collider.gameObject.name);
        DamageSource damage = collider.gameObject.GetComponent<DamageSource>();
        if (damage != null)
        {
            health = health - damage.damage;
            if(health <= 0)
            {
                if (this.gameObject.GetComponent<Enemy>() != null)
                {
                    this.gameObject.GetComponent<Enemy>().Death();
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
            
        }
    }
}
