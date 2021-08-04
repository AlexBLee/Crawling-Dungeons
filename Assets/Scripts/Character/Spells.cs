using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    private Player player;

    public List<SpellNew> spells = new List<SpellNew>();
    public SpellFactory spellFactory = new SpellFactory();

    void Start()
    {
        player = GetComponent<Player>();

        spells.Add(spellFactory.GetSpell("ice"));
        spells.Add(spellFactory.GetSpell("fire"));
    }

    public void UnlockSpells()
    {
        foreach (SpellNew spell in spells)
        {
            if (player.level >= spell.LevelRequired)
            {
                spell.unlocked = true;
            }
        }

    }

}
