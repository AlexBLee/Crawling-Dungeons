using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell
{
    public string Name;
    public int Damage;
    public int Cost;
    public int LevelRequired;
    public int TurnsLeft;

    public Spell(SpellInfo spellInfo)
    {
        Name = spellInfo.name;
        Damage = spellInfo.damage;
        Cost = spellInfo.cost;
        LevelRequired = spellInfo.levelRequired;
        TurnsLeft = spellInfo.turnsLeft;
    }

    public void InstantiateSpell(CharacterEntity entity)
    {
        GameObject spellPrefab = GameDatabase.instance.GetSpellPrefab(Name);
        GameObject.Instantiate(spellPrefab, entity.transform.position, Quaternion.identity);
    }

    public virtual void UseSpellEffect(CharacterEntity entity)
    {
    }

    public virtual void UndoEffect(CharacterEntity entity)
    {
    }
}
