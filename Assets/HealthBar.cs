using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Slider healthbarSlider;
    public Gradient healthGradient;
    public Image fill;

    public void SetMaxHealth(int health)
    { 
      healthbarSlider.value = health;
      healthbarSlider.value = health;


        fill.color = healthGradient.Evaluate(1f);
    }
    public void SetHealth(int health)
    { 
        healthbarSlider.value = health;

        fill.color = healthGradient.Evaluate(healthbarSlider.normalizedValue);
    }
}
