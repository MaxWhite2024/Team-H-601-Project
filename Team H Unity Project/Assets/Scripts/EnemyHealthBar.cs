using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Damageable health;
    public void Start()
    {
        this.gameObject.transform.localScale = new Vector3(health.health*2, 1, 0);
       
    }

    private void FixedUpdate()
    {
        this.gameObject.transform.localScale = new Vector3(health.health*2, 1, 0);

    }
}
