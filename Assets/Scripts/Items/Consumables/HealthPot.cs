public class HealthPot : ConsumableItem
{
    public override void ConsumeEffect(CharacterEntity character)
    {
        character.CharacterBattleStats.Heal((int)(character.CharacterBattleStats.MaxHp * percentageAdd), false);
    }

}
