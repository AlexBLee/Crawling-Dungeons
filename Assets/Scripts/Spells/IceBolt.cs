using UnityEngine;

public class IceBolt : Spell
{
    public IceBolt(SpellInfo spellInfo) : base(spellInfo)
    {
    }
    
    public override void UseSpellEffect(CharacterEntity entity)
    {
        base.UseSpellEffect(entity);

        entity.def -= 5;
        entity.gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }

    public override void UndoEffect(CharacterEntity entity)
    {
        entity.def += 5;
        entity.gameObject.GetComponent<Renderer>().material.color = Color.white;
    }
}
