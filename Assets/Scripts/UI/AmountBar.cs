using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmountBar : MonoBehaviour
{
    [SerializeField]
    private Image bar;

    [SerializeField]
    private TextMeshProUGUI txtAmount;

    [SerializeField]
    private int min, max;

    private int mCurrentValue;
    private float mPercentage;

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
