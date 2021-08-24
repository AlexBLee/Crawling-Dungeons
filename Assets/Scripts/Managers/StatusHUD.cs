public class StatusHUD : HUDMenu
{
    public AmountBar healthBar;
    public AmountBar manaBar;

    public void UpdateUIHealth()
    {
        healthBar.SetAmount(player.hp, player.maxHP);
    }

    public void UpdateUIMana()
    {
        manaBar.SetAmount(player.mp, player.maxMP);
    }
}
