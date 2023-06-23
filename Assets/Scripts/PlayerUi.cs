using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
    public Slider healthSlider;
    public TMPro.TextMeshProUGUI livesCount;

    //Sets current value of health bar
    public void SetHealth(float health)
    {
        healthSlider.value = health;
    }

    //Sets max value of health bar
    public void SetMaxHealth(float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
    }

    public void SetLivesCount(int lifecount)
    {
        string strCount = lifecount.ToString();
        livesCount.text = strCount;
    }
}
