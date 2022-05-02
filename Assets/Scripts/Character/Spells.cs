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
            spellStateInfo.Add(spellName, new SpellStateInfo(spellName));
        }
    }

    public List<string> GetUnlockedSpells()
    {
        List<string> unlockedSpells = new List<string>();

        foreach (string spellName in GameConstants.PlayerAvailableSpells)
        {
            if (spellStateInfo[spellName].GetUnlockedState())
            {
                unlockedSpells.Add(spellName);
            }
        }

        return unlockedSpells;
    }

    public void UnlockSpells(int playerLevel)
    {
        foreach (string spellName in GameConstants.PlayerAvailableSpells)
        {
            Spell spell = SpellFactory.GetSpell(spellName);

            if (playerLevel >= spell.LevelRequired)
            {
                spellStateInfo[spell.Name].SetUnlocked();
            }
        }
    }

}
