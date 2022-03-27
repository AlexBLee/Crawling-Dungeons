using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PotionHUD : HUDMenu
{
    [SerializeField] private Player player;
    public List<PotionUI> potionUIList;

    public void Start()
    {
        InitializePotionButtons();
    }

    public override void Show()
    {
        base.Show();
        DisplayPotionAmount();
    }

    private void InitializePotionButtons()
    {
        List<ConsumableItem> consumableItems = player.inventory.items
                                                .Where(item => item is ConsumableItem)
                                                .Cast<ConsumableItem>().ToList();

        for (int i = 0; i < consumableItems.Count; i++)
        {
            ConsumableItem potion = consumableItems[i];
            PotionUI potionUI = potionUIList[i];

            if (potion != null)
            {
                potionUI.button.onClick.AddListener(() => 
                {
                    player.inventory.ConsumeItem(player.inventory.items.IndexOf(potion), player);
                });

                potionUI.item = potion;
            }
        }

        DisplayPotionAmount();
    }

    private void DisplayPotionAmount()
    {
        foreach (PotionUI potionUI in potionUIList)
        {
            if (potionUI.item != null)
            {
                potionUI.button.interactable = potionUI.item.amount != 0;
                potionUI.text.text = string.Format(DisplayStrings.PotionFormatString, potionUI.item.itemName, potionUI.item.amount.ToString());

            }
            else
            {
                potionUI.button.interactable = false;
                potionUI.text.text = DisplayStrings.NotApplicableText;
            }
        }
    }
}
