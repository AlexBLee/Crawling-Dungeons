using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : EquippableItem
{
    public int minDamage;
    public int maxDamage;
    public bool isMagic;

    public override void AddStatsToPlayer(Player player)
    {
        if (isMagic)
        {
            player.magicMinDamage += minDamage;
            player.magicMaxDamage += maxDamage;
        }
        else
        {
            player.minDamage += minDamage;
            player.maxDamage += maxDamage;
        }
    }

    public override void RemoveStatsFromPlayer(Player player)
    {
        if (isMagic)
        {
            player.magicMinDamage -= minDamage;
            player.magicMaxDamage -= maxDamage;
        }
        else
        {
            player.minDamage -= minDamage;
            player.maxDamage -= maxDamage;
        }
    }

}
