public class FireBolt : Spell
{    
    public FireBolt(SpellInfo spellInfo) : base(spellInfo)
    {
    }

    public override void UseSpellEffect(CharacterEntity entity)
    {
        base.UseSpellEffect(entity);
        entity.CharacterBattleStats.Def -= 3;
    }
}
