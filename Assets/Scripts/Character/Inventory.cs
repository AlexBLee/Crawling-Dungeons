using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public enum EquipSlot
    {
        Helmet,
        Upper,
        RightHand,
        LeftHand,
        Lower,
        Boots
    }
    
    private Player player;

    public List<EquippableItem> equips = new List<EquippableItem>();
    public List<Item> items = new List<Item>();
    public int gold;

    public ShopDisplay shopDisplay;
    public InventoryDisplay inventoryDisplay;
    public ItemPopup itemPopup;

    private void Start()
    {
        player = GetComponent<Player>();

        InitializeInventory();
        ApplyItemsFrom(GameManager.instance.playerStats);
        UpdateItemStats();
    }

    public void InitializeInventory()
    {
        const int EquipSlotCount = 6;
        const int ItemSlotCount = 12;

        equips = new List<EquippableItem>(new EquippableItem[EquipSlotCount]);
        items = new List<Item>(new Item[ItemSlotCount]);
        gold = 0;
    }

    public void ApplyItemsFrom(Player otherPlayer)
    {
        items = otherPlayer.inventory.items;
        equips = otherPlayer.inventory.equips;
        gold = otherPlayer.inventory.gold;
    }

    public void ApplyItemsTo(Player otherPlayer)
    {
        otherPlayer.inventory.items = items;
        otherPlayer.inventory.equips = equips;
        otherPlayer.inventory.gold = gold;
    }

    // -----------------------------------------------------------------------------

    public int AddItem(Item item)
    {
        int index = LookForFreeInventorySpace();

        if (item is ConsumableItem && IsItemExisting(item.itemName))
        {
            int itemIndex = items.IndexOf(item);

            items[itemIndex].amount++;
            inventoryDisplay.UpdateItemAmount(itemIndex);
            return 0;
        }

        item.amount++;
        items[index] = item;
        inventoryDisplay.AddItemImage(item,index);
        return index;
    }

    public void BuyItem(Item item, bool wantToEquip)
    {
        if(gold >= item.cost)
        {
            AudioManager.Instance.PlaySound("Buy");
            gold -= item.cost;
            shopDisplay.UpdateGold();
            Debug.Log("Bought " + item.itemName);
            
            if(wantToEquip)
            {
                EquippableItem tempItem = (EquippableItem)item;
                EquipItem(AddItem(tempItem), tempItem.itemType);
            }
            else
            {
                AddItem(item);
            }
        }
        else
        {
            Debug.Log("Not enough money!");
        }
            
    }

    public void SellItem(int index)
    {
        Item item = items[index];
        AudioManager.Instance.PlaySound("Buy");

        const float SellFactor = 0.5f;
        int sellPrice = (int)((float)item.cost * SellFactor);
        gold += sellPrice;

        if(item is ConsumableItem && item.amount > 1)
        {
            ConsumableItem consumableItem = (ConsumableItem)item;
            consumableItem.amount--;
            inventoryDisplay.UpdateItemAmount(index);
        }
        else
        {
            RemoveItem(index, true);
        }

        shopDisplay.UpdateGold();
    }

    public void RemoveItem(int index, bool selling)
    {
        // selling bool is only to differ between sounds
        if(!selling)
        {
            AudioManager.Instance.PlaySound("Trash");
        }
        itemPopup.gameObject.SetActive(false);
        items[index] = null;

        inventoryDisplay.UpdateItemAmount(index);
        inventoryDisplay.RemoveItemImage(index);
    }

    public void EquipItem(int invIndex, int equipIndex)
    {
        itemPopup.gameObject.SetActive(false);

        EquippableItem itemToEquip = (EquippableItem)inventoryDisplay.items[invIndex];

        if (equips[equipIndex] != null)
        {
            EquippableItem equippedItem = equips[equipIndex];
            AddItem(equips[equipIndex]);

            equippedItem.RemoveStatsFromPlayer(player);
        }

        equips[equipIndex] = itemToEquip;

        itemToEquip.AddStatsToPlayer(player);
        inventoryDisplay.AddEquippedItemImage(equips[equipIndex], equips[equipIndex].itemType);

        inventoryDisplay.items[invIndex] = null;

        RemoveItem(invIndex, false);
    }
    
    public void UnequipItem(int index)
    {
        itemPopup.gameObject.SetActive(false);

        EquippableItem equippableItem = equips[index];

        inventoryDisplay.equippedItems[index] = null;
        inventoryDisplay.RemoveEquippedItemImage(index);

        equips[index] = null;

        equippableItem.RemoveStatsFromPlayer(player);

        AddItem(equippableItem);
    }

    public void ConsumeItem(int index, CharacterEntity character)
    {
        Debug.Log(index);
        // if 0, then display no more potions left!
        ConsumableItem consumable = (ConsumableItem)items[index];

        consumable.amount--;
        consumable.ConsumeEffect(character);
        
        if(consumable.amount == 0)
        {
            items[index] = null;
        }
    }

    // -----------------------------------------------------------------------------

    public void UpdateItemStats()
    {
        for(int i = 0; i < equips.Count; i++)
        {
            if(equips[i] != null)
            {
                equips[i].AddStatsToPlayer(player);
            }
        }
    }

    // -----------------------------------------------------------------------------

    public void SwapItem(int indexA, int indexB)
    {
        Item temp = items[indexA];
        items[indexA] = items[indexB];
        items[indexB] = temp;
        inventoryDisplay.SwapItem(indexA,indexB);
    }

    public void GainMoney(int amount)
    {
        gold += amount;
        inventoryDisplay.goldText.text = gold.ToString();
    }

    public int LookForFreeInventorySpace()
    {
        int spot = 0;

        for(int i = 0; i <= items.Count; i++)
        {
            if(items[i] == null)
            {
                spot = i;
                break;
            }
        }
        return spot;
    }

    public bool IsItemExisting(string itemName)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
            {
                if (items[i].itemName == itemName)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public List<EquippableItem> GetEquipList()
    {
        return equips;
    }
}
