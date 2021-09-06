﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorItem : EquippableItem
{
    public int defense;

    public override void AddStatsToPlayer(Player player)
    {
        player.def += defense;
    }

    public override void RemoveStatsFromPlayer(Player player)
    {
        player.def -= defense;
    }
}
