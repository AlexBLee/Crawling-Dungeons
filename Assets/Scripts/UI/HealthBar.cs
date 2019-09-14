using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image imgHealthBar;
    public TextMeshProUGUI txtHealth;
    public int min;
    public int max;
    public int mCurrentValue;
    public float mPercentage;

    public void SetHealth(int health, int maxHealth)
    {
        if (health != mCurrentValue)
        {
            if (max - min == 0)
            {
                mCurrentValue = 0;
                mPercentage = 0;
            }
            else
            {
                mCurrentValue = health;
                mPercentage = (float)mCurrentValue / (float)(max - min);
            }

            imgHealthBar.fillAmount = mPercentage;
            txtHealth.text = "HP: " + health + "/" + maxHealth;
        }
    }

    public float CurrentPercentage
    {
        get { return mPercentage; }
    }

    public float CurrentValue
    {
        get { return mCurrentValue; }
    }

}
