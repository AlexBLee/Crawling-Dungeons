using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    private Player player;

    public Vector3 spellPosition;
    public List<Spell> spells;
    public Dictionary<Spell, bool> spellList;

    void Start()
    {
        player = GetComponent<Player>();

        spellList = new Dictionary<Spell, bool>();
        foreach(Spell spell in spells)
        {
            spellList.Add(spell, false);
        }
    }

    public void UnlockSpells()
    {
        if(player.level >= 1) { spellList[spells[0]] = true; }

        if(player.level >= 5) { spellList[spells[1]] = true; }

        if(player.level >= 7) { spellList[spells[2]] = true; }

        if(player.level >= 12) { spellList[spells[3]] = true; }

        if(player.level >= 20) { spellList[spells[4]] = true; }

        if(player.level >= 25) { spellList[spells[5]] = true; }

    }

}
