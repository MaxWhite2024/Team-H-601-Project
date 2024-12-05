using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Gradient healthGradient;
    public Image fill;
    public Damageable playerDamageable;

    public void Start()
    {
        playerDamageable = GameObject.Find("Players").GetComponent<Damageable>();

        SetMaxHealth(playerDamageable.maxHealth, playerDamageable.health); 
    }
    public void Update()
    {
        SetHealth(playerDamageable.health);
    }
    public void SetHealth(int health)
    {
        healthSlider.value = health;

        fill.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }

    public void SetMaxHealth(int maxHealth, int health)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;

        fill.color = healthGradient.Evaluate(1f);
    }
}
