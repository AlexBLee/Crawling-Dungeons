using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{    
    public float percentageAdd = 0.0f;
    
    public virtual void ConsumeEffect(CharacterEntity character) {}
}
