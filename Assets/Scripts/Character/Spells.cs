using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    private List<SpellInfo> spells = new List<SpellInfo>();
    private Dictionary<string, SpellStateInfo> spellStateInfo = new Dictionary<string, SpellStateInfo>();

    private void Start()
    {
        foreach (string spellName in GameConstants.PlayerAvailableSpells)
        {
            spells.Add(GameDatabase.instance.GetSpellData(spellName));
            spellStateInfo.Add(spellName, new SpellStateInfo(spellName));
        }
    }

    public List<SpellInfo> GetUnlockedSpells()
    {
        List<SpellInfo> unlockedSpells = new List<SpellInfo>();

        foreach (SpellInfo spell in spells)
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
        foreach (SpellInfo spell in spells)
        {
            if (playerLevel >= spell.levelRequired)
            {
                spellStateInfo[spell.name].SetUnlocked();
            }
        }
    }

}
