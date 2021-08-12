using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPot : ConsumableItem
{
    public override void ConsumeEffect(CharacterEntity character)
    {
        character.Heal((int)(character.maxHP * percentageAdd), false);
    }

}
