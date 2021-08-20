using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    private List<Spell> spells = new List<Spell>();
    private Dictionary<string, SpellStateInfo> spellStateInfo = new Dictionary<string, SpellStateInfo>();

    private void Start()
    {
        foreach (string spellName in GameConstants.PlayerAvailableSpells)
        {
            spells.Add(GameDatabase.instance.GetSpellData(spellName));
            spellStateInfo.Add(spellName, new SpellStateInfo(spellName));
        }
    }

    public List<Spell> GetUnlockedSpells()
    {
        List<Spell> unlockedSpells = new List<Spell>();

        foreach (Spell spell in spells)
        {
            if (spellStateInfo[spell.name].GetUnlockedState())
            {
                unlockedSpells.Add(spell);
            }
        }

        return unlockedSpells;
    }

    public void UnlockSpells(int playerLevel)
    {
        foreach (Spell spell in spells)
        {
            if (playerLevel >= spell.levelRequired)
            {
                spellStateInfo[spell.name].SetUnlocked();
            }
        }
    }

}
