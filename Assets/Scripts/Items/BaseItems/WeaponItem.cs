using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon")]
public class WeaponItem : EquippableItem
{
    public int minDamage;
    public int maxDamage;
    public bool isMagic;

}
