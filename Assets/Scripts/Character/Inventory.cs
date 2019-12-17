using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Player player;

    public List<EquippableItem> equips;
    public List<Item> items;
    public int gold;

    public ShopDisplay shopDisplay;
    public InventoryDisplay inventoryDisplay;
    public ItemPopup itemPopup;

    private void Awake() 
    {
        player = GetComponent<Player>();

        ApplyItemsFrom(GameManager.instance.playerStats);
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

    public void AddItem(Item item)
    {
        int index = LookForFreeInventorySpace();

        items[index] = item;
        inventoryDisplay.AddItemImage(item,index);
    }

    public void BuyItem(Item item)
    {
        if(gold >= item.cost)
        {
            AudioManager.Instance.Play("Buy");
            gold -= item.cost;
            shopDisplay.UpdateGold();
            Debug.Log("Bought " + item.itemName);
            AddItem(item);
        }
        else
        {
            Debug.Log("Not enough money!");
        }
            
    }

    public void RemoveItem(int index)
    {
        AudioManager.Instance.Play("Trash");
        itemPopup.gameObject.SetActive(false);
        items[index] = null;
        inventoryDisplay.RemoveItemImage(index);
    }

    public void EquipItem(int invIndex, int equipIndex)
    {
        itemPopup.gameObject.SetActive(false);

        EquippableItem tempItem = (EquippableItem)inventoryDisplay.items[invIndex];

        if(equips[equipIndex] != null)
        {
            EquippableItem equippedItem = equips[equipIndex];
            AddItem(equips[equipIndex]);

            RemoveItemStats(equippedItem);
        }

        AddItemStats(tempItem);

        equips[equipIndex] = tempItem;
        inventoryDisplay.AddEquippedItemImage(equips[equipIndex], equips[equipIndex].itemType);

        inventoryDisplay.items[invIndex] = null;

        RemoveItem(invIndex);
    }
    
    public void UnequipItem(int index)
    {
        itemPopup.gameObject.SetActive(false);

        Item tempItem = equips[index];

        inventoryDisplay.equippedItems[index] = null;
        inventoryDisplay.RemoveEquippedItemImage(index);

        equips[index] = null;

        // For swapping equips
        RemoveItemStats(tempItem);

        AddItem(tempItem);
    }

    public void AddItemStats(Item item)
    {
        if(item is ArmorItem)
        {
            AudioManager.Instance.Play("ArmorEquip");
            ArmorItem armor = (ArmorItem)item;
            player.def += armor.defense;
        }
        else if(item is WeaponItem)
        {
            AudioManager.Instance.Play("WeaponEquip");
            WeaponItem wpn = (WeaponItem)item;
            if(wpn.isMagic)
            {
                player.magicMinDamage += wpn.minDamage;
                player.magicMaxDamage += wpn.maxDamage;
            }
            else
            {
                player.minDamage += wpn.minDamage;
                player.maxDamage += wpn.maxDamage;
            }
        }
    }

    public void RemoveItemStats(Item item)
    {
        if(item is ArmorItem)
        {
            ArmorItem armor = (ArmorItem)item;
            player.def -= armor.defense;
        }
        else if(item is WeaponItem)
        {
            WeaponItem wpn = (WeaponItem)item;
            if(wpn.isMagic)
            {
                player.magicMinDamage -= wpn.minDamage;
                player.magicMaxDamage -= wpn.maxDamage;
            }
            else
            {
                player.minDamage -= wpn.minDamage;
                player.maxDamage -= wpn.maxDamage;
            }
        }
    }

    public void UpdateItemStats()
    {
        for(int i = 0; i < equips.Count; i++)
        {
            if(equips[i] != null)
            {
                if(equips[i] is ArmorItem)
                {
                    ArmorItem tempItem = (ArmorItem)equips[i];
                    player.def += tempItem.defense;
                }
                else if(equips[i] is WeaponItem)
                {
                    WeaponItem tempItem = (WeaponItem)equips[i];
                    if(tempItem.isMagic)
                    {
                        player.magicMinDamage += tempItem.minDamage;
                        player.magicMaxDamage += tempItem.maxDamage;
                    }
                    else
                    {
                        player.minDamage += tempItem.minDamage;
                        player.maxDamage += tempItem.maxDamage;
                    }
                }
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
}
