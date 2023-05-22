using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Slider healthbarSlider;
    public Gradient healthGradient;
    public Image fill;

    public void SetMaxHealth(int maxhealth)
    { 
      healthbarSlider.value = maxhealth;


      fill.color = healthGradient.Evaluate(1f);
    }
    public void SetHealth(float health)
    {
        healthbarSlider.value = health;
        fill.color = healthGradient.Evaluate(healthbarSlider.normalizedValue);
    }
}
