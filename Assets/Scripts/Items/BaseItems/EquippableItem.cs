using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippableItem : Item
{
    public float durability;
    public int itemType;
    
    public virtual void AddStatsToPlayer(Player player)
    {
    }

    public virtual void RemoveStatsFromPlayer(Player player)
    {
    }
    

}
