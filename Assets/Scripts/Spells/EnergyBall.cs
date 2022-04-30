using UnityEngine;

public class EnergyBall : SpellInfo
{
    public override void UseSpellEffect(CharacterEntity entity)
    {
        GameObject spellPrefab = GameDatabase.instance.GetSpellPrefab(name);
        GameObject.Instantiate(spellPrefab, entity.target.transform.position, Quaternion.identity);
    }
}
