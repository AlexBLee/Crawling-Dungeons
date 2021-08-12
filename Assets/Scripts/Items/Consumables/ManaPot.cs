using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPot : ConsumableItem
{
    public override void ConsumeEffect(CharacterEntity character)
    {
        character.RestoreMP((int)(character.maxMP * percentageAdd), false);
    }

}
