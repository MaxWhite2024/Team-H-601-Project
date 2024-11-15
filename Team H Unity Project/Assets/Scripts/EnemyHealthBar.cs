using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{

    public Damageable health;
    [SerializeField] private float originalXScaling;


    public void Start()
    {
        originalXScaling = gameObject.transform.localScale.x;
        gameObject.transform.localScale = new Vector3(((float)health.health / (float)health.maxHealth) * originalXScaling, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
       
    }

    private void FixedUpdate()
    {
        gameObject.transform.localScale = new Vector3(((float)health.health / (float)health.maxHealth) * originalXScaling, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
       // Debug.Log("health = " + health.health + ". maxHleath = " + health.maxHealth); 
    }
}
