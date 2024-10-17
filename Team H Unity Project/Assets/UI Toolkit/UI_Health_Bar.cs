using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Gradient healthGradient;
    public Image fill;

    public GameObject Player;
    public void SetMaxHealth(int health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;

        fill.color = healthGradient.Evaluate(1f);
    }
    public void Start()
    {
        int PlayerHealth = 0;
        PlayerHealth=Player.GetComponent<Damageable>().health;
        SetMaxHealth(PlayerHealth);
      
    }
    public void Update()
    {
        int PlayerHealth = 0;
        PlayerHealth = Player.GetComponent<Damageable>().health;
        SetHealth(PlayerHealth);
    }
    public void SetHealth(int health)
    {
        healthSlider.value = health;

        fill.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }
}
