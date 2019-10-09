using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;


public class Player : CharacterEntity
{
    public HealthBar healthBar;
    public HealthBar manaBar;

    public Vector3 pos;
    public Vector2 enemyPosition;

    private bool buttonPressed;
    
    private StatDisplayer statDisplayer;    

    // Equipped Items
    public List<Spell> spellList;
    public List<Item> itemList;
    public EquippableItem tempItem;

    public Helmet helmet;
    public Upper upper;
    public Lower lower;
    public RightHand rightHand;
    public LeftHand leftHand;

    public List<AddRemoveStat> addRemoves;

    public int gold;


    private void Start()
    {
        ApplyStatsFrom(GameManager.instance.playerStats);
        pos = transform.position;
        
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
        uiManager.HideMagicList();
        
        Debug.Log("Casted: " + spell.name);
        mp -= spell.cost;
        manaBar.SetHealth(mp, maxMP);

        attacking = true;
        RangedAttack();
        Instantiate(spell.effect, target.transform.position, Quaternion.identity);

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
            yield return new WaitForSeconds(0.15f);
            healthBar.SetHealth(hp,maxHP);
            manaBar.SetHealth(mp,maxMP);
            infoText.text = "Level up!";
            Instantiate(infoText, transform.position, Quaternion.identity);

            exp += extraXP;
        }

    }

    public void UpdateUIHealth()
    {
        healthBar.SetHealth(hp, maxHP);
    }

    public IEnumerator NextBattle()
    {
        yield return new WaitForSeconds(3);
        anim.SetBool("Run", true);
        battleManager.battleDone = true;
        battleManager.ToggleNextBattle();
    }

    public void DeactivateAdders()
    {
        for(int i = 0; i < addRemoves.Count; i++)
        {
            addRemoves[i].gameObject.SetActive(false);
        }
    }

    public void ActivateAdders()
    {
        for(int i = 0; i < addRemoves.Count; i++)
        {
            addRemoves[i].gameObject.SetActive(true);
        }
    }

    #endregion

    // -----------------------------------------------------------------------------

    #region Inventory

    // public void InitializeInventory()
    // {
    //     itemList.Clear();
    // }

    // public void AddItem(Item item)
    // {
    //     // For item getting added from some source
    //     int index = LookForFreeInventorySpace();

    //     itemList[index] = item;
    //     invDisplay.AddItemImage(item,index);
    // }

    // public void AddItem(Item item, int index)
    // {
    //     // For item slot switching
    //     itemList[index] = item;
    //     invDisplay.AddItemImage(item,index);
    // }

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

    // public int LookForFreeInventorySpace()
    // {
    //     int spot = 0;

    //     for(int i = 0; i <= itemList.Count; i++)
    //     {
    //         if(itemList[i] == null)
    //         {
    //             spot = i;
    //             break;
    //         }
    //     }
    //     return spot;
    // }

    #endregion
    


}
