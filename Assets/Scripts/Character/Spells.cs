using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    private Player player;

    public Vector3 spellPosition;
    public List<SpellNew> spells = new List<SpellNew>();
    public Dictionary<SpellNew, bool> spellList;

    public SpellFactory spellFactory = new SpellFactory();

    void Start()
    {
        player = GetComponent<Player>();

        spellList = new Dictionary<SpellNew, bool>();

        spells.Add(spellFactory.GetSpell("ice"));
        spells.Add(spellFactory.GetSpell("fire"));

        foreach(SpellNew spell in spells)
        {
            spellList.Add(spell, false);
        }
    }

    public void UnlockSpells()
    {
        foreach(var spell in spellList)
        {
            if (player.level >= spell.Key.LevelRequired)
            {
                spellList[spell.Key] = true;
            }
        }

    }

}
