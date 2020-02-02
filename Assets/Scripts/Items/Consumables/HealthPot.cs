using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/ConsumableItem/Health Pot")]
public class HealthPot : ConsumableItem
{
    public float hpPercentageAdd;

    public override void ConsumeEffect(CharacterEntity character)
    {
        character.Heal((int)(character.maxHP * hpPercentageAdd), false);
    }

}
