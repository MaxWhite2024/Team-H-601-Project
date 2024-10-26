using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Damageable health;
    private float originalXScaling;

    public void Start()
    {
        gameObject.transform.localScale = new Vector3(health.health * gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        originalXScaling = gameObject.transform.localScale.x;
    }

    private void FixedUpdate()
    {
        gameObject.transform.localScale = new Vector3(health.health * originalXScaling, gameObject.transform.localScale.y, gameObject.transform.localScale.z);

    }
}
