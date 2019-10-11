using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmountBar : MonoBehaviour
{
    public Image bar;
    public TextMeshProUGUI txtAmount;
    public int min;
    public int max;
    public int mCurrentValue;
    public float mPercentage;

    public void SetAmount(int minAmount, int maxAmount)
    {
        if (minAmount != mCurrentValue)
        {
            if (max - min == 0)
            {
                mCurrentValue = 0;
                mPercentage = 0;
            }
            else
            {
                if(max != maxAmount)
                {
                    max = maxAmount;
                }
                mCurrentValue = minAmount;
                mPercentage = (float)mCurrentValue / (float)(max - min);
            }

            bar.fillAmount = mPercentage;
            txtAmount.text = minAmount + "/" + maxAmount;
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
