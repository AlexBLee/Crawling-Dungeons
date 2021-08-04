using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    private List<SpellNew> spells = new List<SpellNew>();
    private SpellFactory spellFactory = new SpellFactory();

    private void Start()
    {
        foreach (string spellName in GameConstants.PlayerAvailableSpells)
        {
            spells.Add(spellFactory.GetSpell(spellName));
        }
    }

    public List<SpellNew> GetSpellList()
    {
        return spells;
    }

    public void UnlockSpells(int playerLevel)
    {
        foreach (SpellNew spell in spells)
        {
            if (playerLevel >= spell.LevelRequired)
            {
                spell.unlocked = true;
            }
        }
    }

}
