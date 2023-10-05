using UnityEngine;

public class StatusHUD : HUDMenu
{
    [SerializeField] private CharacterEntity _characterEntity;
    public AmountBar healthBar;
    public AmountBar manaBar;

    public CharacterEntity CharacterEntity => _characterEntity;

    public void SetCharacter(CharacterEntity entity)
    {
        _characterEntity = entity;
    }

    public void UpdateUIHealth()
    {
        healthBar.SetAmount(_characterEntity.CharacterBattleStats.Hp, _characterEntity.CharacterBattleStats.MaxHp);
    }

    public void UpdateUIMana()
    {
        manaBar.SetAmount(_characterEntity.CharacterBattleStats.Mp, _characterEntity.CharacterBattleStats.MaxMp);
    }

    public void UpdateAllBars()
    {
        UpdateUIHealth();
        UpdateUIMana();
    }
}
