using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    private Player player;

    public Vector3 spellPosition;
    public List<Spell> spells;
    public Dictionary<Spell, bool> spellList;

    private const int first_spellUnlockLevel = 1;
    private const int second_spellUnlockLevel = 2;
    private const int third_spellUnlockLevel = 3;
    private const int fourth_spellUnlockLevel = 4;
    private const int fifth_spellUnlockLevel = 5;
    private const int sixth_spellUnlockLevel = 6;



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
        if(player.level >= first_spellUnlockLevel) { spellList[spells[0]] = true; }

        if(player.level >= second_spellUnlockLevel) { spellList[spells[1]] = true; }

        if(player.level >= third_spellUnlockLevel) { spellList[spells[2]] = true; }

        if(player.level >= fourth_spellUnlockLevel) { spellList[spells[3]] = true; }

        if(player.level >= fifth_spellUnlockLevel) { spellList[spells[4]] = true; }

        if(player.level >= sixth_spellUnlockLevel) { spellList[spells[5]] = true; }

    }

}
