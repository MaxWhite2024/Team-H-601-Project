using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Gradient healthGradient;
    public Image fill;

    private Damageable playerDamageable;
    public void SetMaxHealth(int health, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;

        fill.color = healthGradient.Evaluate(1f);
    }
    public void Start()
    {
        playerDamageable = GameObject.Find("Players").GetComponent<Damageable>();

        SetMaxHealth(playerDamageable.health, playerDamageable.maxHealth);
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
}
