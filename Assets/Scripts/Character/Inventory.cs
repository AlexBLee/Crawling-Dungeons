using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Player player;

    public List<EquippableItem> equipInventory;
    public List<Item> itemList;
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
        itemList = otherPlayer.inventory.itemList;
        equipInventory = otherPlayer.inventory.equipInventory;
        gold = otherPlayer.inventory.gold;
    }

    public void ApplyItemsTo(Player otherPlayer)
    {
        otherPlayer.inventory.itemList = itemList;
        otherPlayer.inventory.equipInventory = equipInventory;
        otherPlayer.inventory.gold = gold;
    }

    public void AddItem(Item item)
    {
        int index = LookForFreeInventorySpace();

        itemList[index] = item;
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
        itemList[index] = null;
        inventoryDisplay.RemoveItemImage(index);
    }

    public void EquipItem(int invIndex, int equipIndex)
    {
        itemPopup.gameObject.SetActive(false);

        EquippableItem tempItem = (EquippableItem)inventoryDisplay.items[invIndex];

        if(equipInventory[equipIndex] != null)
        {
            EquippableItem equippedItem = equipInventory[equipIndex];
            AddItem(equipInventory[equipIndex]);

            RemoveItemStats(equippedItem);
        }

        AddItemStats(tempItem);

        equipInventory[equipIndex] = tempItem;
        inventoryDisplay.AddEquippedItemImage(equipInventory[equipIndex], equipInventory[equipIndex].itemType);

        inventoryDisplay.items[invIndex] = null;

        RemoveItem(invIndex);
    }
    
    public void UnequipItem(int index)
    {
        itemPopup.gameObject.SetActive(false);

        Item tempItem = equipInventory[index];

        inventoryDisplay.equippedItems[index] = null;
        inventoryDisplay.RemoveEquippedItemImage(index);

        equipInventory[index] = null;

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
        for(int i = 0; i < equipInventory.Count; i++)
        {
            if(equipInventory[i] != null)
            {
                if(equipInventory[i] is ArmorItem)
                {
                    ArmorItem tempItem = (ArmorItem)equipInventory[i];
                    player.def += tempItem.defense;
                }
                else if(equipInventory[i] is WeaponItem)
                {
                    WeaponItem tempItem = (WeaponItem)equipInventory[i];
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
        Item temp = itemList[indexA];
        itemList[indexA] = itemList[indexB];
        itemList[indexB] = temp;
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

        for(int i = 0; i <= itemList.Count; i++)
        {
            if(itemList[i] == null)
            {
                spot = i;
                break;
            }
        }
        return spot;
    }
}
