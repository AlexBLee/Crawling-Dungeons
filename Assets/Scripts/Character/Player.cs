using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;


public class Player : CharacterEntity
{
    public AmountBar healthBar;
    public AmountBar manaBar;

    public Vector3 pos;
    public Vector2 enemyPosition;
    public Vector3 spellPosition;

    private bool buttonPressed;
    
    private StatDisplayer statDisplayer;   

    // Equipped Items
    public List<Spell> spells;
    public Dictionary<Spell, bool> spellList;
    public List<Item> itemList;
    public EquippableItem tempItem;

    public InventoryDisplay inventoryDisplay;

    public Helmet helmet;
    public Upper upper;
    public Lower lower;
    public RightHand rightHand;
    public LeftHand leftHand;
    public Boots boots;


    public int gold;

    private void Awake() {
        spellList = new Dictionary<Spell, bool>();
        foreach(Spell spell in spells)
        {
            spellList.Add(spell, false);
        }

    }


    private void Start()
    {
        ApplyStatsFrom(GameManager.instance.playerStats);
        pos = transform.position;

        UnlockSpells();
        UpdateDamageStats();
    }

    private void Update()
    {
        // --------------------------------------------------------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------------------------------------------------------- //


        if(inBattle && battleManager.playerTurn)
        {
            if(hp <= 0)
            {
                Debug.Log("oof!");
            }
            
            if (target != null)
            {     
                MoveAndAttack(target.transform.position, 1);
            }
            StopAttacking();  
        }

        // --------------------------------------------------------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------------------------------------------------------- //

    }

    // -----------------------------------------------------------------------------

    public void Attack()
    {
        uiManager.DisableButtons();
        initialPos = transform.position;
        targetReached = false;
        attacking = true;
    }

    public void MagicPressed(Spell spell)
    {
        if((mp - spell.cost) < 0)
        {
            Debug.Log("not enough mana!");
            infoText.text = "Not enough mana!";
            Instantiate(infoText, transform.position, Quaternion.identity);
        }
        else
        {
            uiManager.HideMagicList();

            Debug.Log("Casted: " + spell.name);
            additionalDamage = spell.additionalDamage;
            mp -= spell.cost;
            uiManager.UpdateUIMana();

            attacking = true;
            RangedAttack();

            // Asks if the spell is supposed to be spawned firstly near the player or right on top of the enemy.
            if(spell.atPosition)
            {
                Instantiate(spell.effect, spellPosition, Quaternion.identity);            
            }
            else
            {
                Instantiate(spell.effect, target.transform.position, Quaternion.identity);
            }
        }


        

    }

    #region Stats

    public void AddToStat(int number)
    {
        switch (number)
        {
            case 1:
                str++;
                break;
            case 2:
                dex++;
                break;
            case 3:
                intl++;
                break;
            case 4:
                will++;
                break;
            default:
                break;
        }

        statPoints--;
        UpdateDamageStats();
    }

    public void RemoveFromStat(int number)
    {
        switch (number)
        {
            case 1:
                str--;
                break;
            case 2:
                dex--;
                break;
            case 3:
                intl--;
                break;
            case 4:
                will--;
                break;
            default:
                break;
        }

        statPoints++;
        UpdateDamageStats();
    }

    public void RecieveXP(int expRecieved)
    {
        exp += expRecieved;
        infoText.text = "+" + expRecieved.ToString() + " XP";
        Instantiate(infoText, transform.position, Quaternion.identity);
        StartCoroutine(CheckForLevelUp());
        StartCoroutine(NextBattle());

    }

    public IEnumerator CheckForLevelUp()
    {
        while(exp >= expThreshold)
        {
            float extraXP = exp - expThreshold;
            LevelUp();
            UnlockSpells();
            yield return new WaitForSeconds(0.15f);
            healthBar.SetAmount(hp,maxHP);
            manaBar.SetAmount(mp,maxMP);
            infoText.text = "Level up!";
            Instantiate(infoText, transform.position, Quaternion.identity);

            exp += extraXP;
        }

    }

    public void UnlockSpells()
    {
        switch(level)
        {
            case 1:
                spellList[spells[0]] = true;
                break;
            case 5:
                spellList[spells[1]] = true;
                break;
            case 7:
                spellList[spells[2]] = true;
                break;
            case 12:
                spellList[spells[3]] = true;
                break;
            case 20:
                spellList[spells[4]] = true;
                break;
            case 25:
                spellList[spells[5]] = true;
                break;

            default:
                break;
        }
    }

    public IEnumerator NextBattle()
    {
        yield return new WaitForSeconds(3);
        anim.SetBool("Run", true);
        battleManager.battleDone = true;
        battleManager.ToggleNextBattle();
    }


    #endregion

    // -----------------------------------------------------------------------------

    #region Inventory



    public void AddItem(Item item)
    {
        if(gold >= item.cost)
        {
            Debug.Log("Bought " + item.itemName);

            // For item getting added from some source
            int index = LookForFreeInventorySpace();

            itemList[index] = item;
            inventoryDisplay.AddItemImage(item,index);
        }
        else
        {
            Debug.Log("Not enough money!");
        }
            
    }

    // public void RemoveItem(int index)
    // {
    //     itemList[index] = null;
    //     invDisplay.RemoveItemImage(index);
    // }

    // public void EquipItem(Item item, int index)
    // {
    //     // Equipping without anything equipped
    //     if(item is ArmorItem)
    //     {
    //         EquipArmorItem((ArmorItem)item);
    //     }
    //     else if(item is WeaponItem)
    //     {
    //         EquipWeaponItem((WeaponItem)item);
    //     }
    //     UpdateDamageStats();
    //     RemoveItem(index);
    // }

    // public void EquipItem(Item item)
    // {
    //     // Swapping an equipped item
    //     if(item is ArmorItem)
    //     {
    //         EquipArmorItem((ArmorItem)item);
    //     }
    //     else if(item is WeaponItem)
    //     {
    //         EquipWeaponItem((WeaponItem)item);
    //     }
    //     UpdateDamageStats();

    // }

    // public void EquipArmorItem(ArmorItem item)
    // {
    //     if(item is Helmet)
    //     {
    //         helmet = (Helmet)item;
    //         charPanel.AddItem(helmet, charPanel.helmetNumber);
    //     }
    //     else if(item is Upper)
    //     {
    //         upper = (Upper)item;
    //         charPanel.AddItem(upper, charPanel.upperNumber);
    //     }
    //     else if(item is Lower)
    //     {
    //         lower = (Lower)item;
    //         charPanel.AddItem(lower, charPanel.lowerNumber);
    //     }
    //     else if(item is LeftHand)
    //     {
    //         leftHand = (LeftHand)item;
    //         charPanel.AddItem(leftHand, charPanel.leftNumber);
    //     }

    //     def += item.defense;
    // }

    // public void EquipWeaponItem(WeaponItem item)
    // {
    //     rightHand = (RightHand)item;
    //     charPanel.AddItem(rightHand, charPanel.rightNumber);
    //     minDamage += item.minDamage;
    //     maxDamage += item.maxDamage;
    // }
    
    // public void UnequipItem(EquippableItem item)
    // {
    //     // For unequipping only
    //     if(item is ArmorItem)
    //     {
    //         UnequipArmorItem((ArmorItem)item);
    //     }
    //     else if(item is WeaponItem)
    //     {
    //         UnequipWeaponItem((WeaponItem)item);
    //     }

    //     int index = LookForFreeInventorySpace();
        
    //     itemList[index] = item;
    //     invDisplay.AddItemImage(item,index);

    // }

    // public void UnequipItem(EquippableItem item, int index)
    // {
    //     // For swapping equips
    //     if(item is ArmorItem)
    //     {
    //         UnequipArmorItem((ArmorItem)item);
    //     }
    //     else if(item is WeaponItem)
    //     {
    //         UnequipWeaponItem((WeaponItem)item);
    //     }

    //     itemList[index] = item;
    //     invDisplay.AddItemImage(item,index);
    // }

    // public void UnequipItemAndDrop(EquippableItem item)
    // {
    //     // Dropping the item out of the bounds of the UI
    //     if(item is ArmorItem)
    //     {
    //         UnequipArmorItem((ArmorItem)item);
    //     }
    //     else if(item is WeaponItem)
    //     {
    //         UnequipWeaponItem((WeaponItem)item);
    //     }

    //     item = null;
    // }

    // public void UnequipArmorItem(ArmorItem item)
    // {
    //     if(item is Helmet)
    //     {
    //         helmet = null;
    //     }
    //     else if(item is Upper)
    //     {
    //         upper = null;
    //     }
    //     else if(item is Lower)
    //     {
    //         lower = null;
    //     }
    //     else if(item is LeftHand)
    //     {
    //         leftHand = null;
    //     }
    //     def -= item.defense;
    // }

    // public void UnequipWeaponItem(WeaponItem item)
    // {
    //     rightHand = null;
    //     str -= item.minDamage;
    //     str -= item.maxDamage;
    // }

    // // -----------------------------------------------------------------------------

    // public void SwapItem(int indexA, int indexB)
    // {
    //     Item temp = itemList[indexA];
    //     itemList[indexA] = itemList[indexB];
    //     itemList[indexB] = temp;
    //     invDisplay.SwapItem(indexA,indexB);
    // }

    // public void GainMoney(int amount)
    // {
    //     gold += amount;
    //     invDisplay.goldText.text = gold.ToString();
    // }

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

    #endregion
    


}
