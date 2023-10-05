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
            player.CharacterBattleStats.MagicMinDamage += minDamage;
            player.CharacterBattleStats.MagicMaxDamage += maxDamage;
        }
        else
        {
            player.CharacterBattleStats.MinDamage += minDamage;
            player.CharacterBattleStats.MaxDamage += maxDamage;
        }
    }

    public override void RemoveStatsFromPlayer(Player player)
    {
        if (isMagic)
        {
            player.CharacterBattleStats.MagicMinDamage -= minDamage;
            player.CharacterBattleStats.MagicMaxDamage -= maxDamage;
        }
        else
        {
            player.CharacterBattleStats.MinDamage -= minDamage;
            player.CharacterBattleStats.MaxDamage -= maxDamage;
        }
    }

}
