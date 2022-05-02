using UnityEngine;

public class EnergyBall : Spell
{
    public EnergyBall(SpellInfo spellInfo) : base(spellInfo)
    {
    }

    public override void UseSpellEffect(CharacterEntity entity)
    {
        GameObject spellPrefab = GameDatabase.instance.GetSpellPrefab(Name);
        GameObject.Instantiate(spellPrefab, entity.target.transform.position, Quaternion.identity);
    }
}
