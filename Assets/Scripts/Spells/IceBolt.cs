using UnityEngine;

public class IceBolt : Spell
{
    public IceBolt(SpellInfo spellInfo) : base(spellInfo)
    {
    }
    
    public override void UseSpellEffect(CharacterEntity entity)
    {
        base.UseSpellEffect(entity);

        entity.CharacterBattleStats.Def -= 5;
        entity.gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }

    public override void UndoEffect(CharacterEntity entity)
    {
        entity.CharacterBattleStats.Def += 5;
        entity.gameObject.GetComponent<Renderer>().material.color = Color.white;
    }
}
