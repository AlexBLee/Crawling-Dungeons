public class ManaPot : ConsumableItem
{
    public override void ConsumeEffect(CharacterEntity character)
    {
        character.CharacterBattleStats.RestoreMp((int)(character.CharacterBattleStats.MaxMp * percentageAdd), false);
    }

}
