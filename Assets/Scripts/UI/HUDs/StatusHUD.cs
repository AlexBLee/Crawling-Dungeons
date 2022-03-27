public class StatusHUD : HUDMenu
{
    public CharacterEntity characterEntity;
    public AmountBar healthBar;
    public AmountBar manaBar;

    public void UpdateUIHealth()
    {
        healthBar.SetAmount(characterEntity.hp, characterEntity.maxHP);
    }

    public void UpdateUIMana()
    {
        manaBar.SetAmount(characterEntity.mp, characterEntity.maxMP);
    }

    public void UpdateAllBars()
    {
        UpdateUIHealth();
        UpdateUIMana();
    }
}
