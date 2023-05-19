using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider StaminaSlider;
    public Gradient StaminaGradient;
    public Image fill;

    public void SetMaxStamina(int maxStamina)
    {
        StaminaSlider.value = maxStamina;
        StaminaSlider.value = maxStamina;


        fill.color = StaminaGradient.Evaluate(1f);
    }
    public void SetStamina(float stamina)
    {
        StaminaSlider.value = stamina;

        fill.color = StaminaGradient.Evaluate(StaminaSlider.normalizedValue);
    }
}
