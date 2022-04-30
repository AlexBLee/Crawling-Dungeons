public class FireBolt : SpellInfo
{
    public override void UseSpellEffect(CharacterEntity entity)
    {
        base.UseSpellEffect(entity);
        entity.def -= 3;
    }
}
