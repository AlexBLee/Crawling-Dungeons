using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Empty class only to specify the type of equip.
[CreateAssetMenu(menuName = "Item/Armor/LeftHand")]
public class LeftHand : ArmorItem
{
    public float percentageOfCritBlocked;

    public int ReduceCrit(float value)
    {
        return (int)(value * (1.00 - percentageOfCritBlocked));
    }
}
