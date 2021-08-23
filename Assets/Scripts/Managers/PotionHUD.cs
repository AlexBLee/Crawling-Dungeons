using System.Collections.Generic;

public class PotionHUD : HUDMenu
{
    public List<PotionUI> potionUIList;

    public void Start()
    {
        InitializePotionButtons();
    }

    public override void Show()
    {
        gameObject.SetActive(true);
        DisplayPotionAmount();
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    private void InitializePotionButtons()
    {
        List<ConsumableItem> consumableItems = new List<ConsumableItem>();

        foreach (ConsumableItem potion in player.inventory.items)
        {
            consumableItems.Add(potion);
        }

        for (int i = 0; i < potionUIList.Count; i++)
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
                potionUI.button.interactable = potionUI.item.amount == 0 ? false : true;
                potionUI.text.text = potionUI.item.itemName + " x" + potionUI.item.amount.ToString();

            }
            else
            {
                potionUI.button.interactable = false;
                potionUI.text.text = "N/A";
            }
        }
    }
}
