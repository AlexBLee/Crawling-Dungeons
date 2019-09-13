using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;


public class Player : CharacterEntity
{

    public Vector3 pos;
    public Vector2 enemyPosition;
    public Button meleeAttackButton;
    public Button magicAttackButton;
    public Button potionButton;

    private bool buttonPressed;
    
    private InventoryDisplay invDisplay;
    private CharacterPanel charPanel;
    private StatDisplayer statDisplayer;    

    // Equipped Items
    public List<Item> itemList;
    public EquippableItem tempItem;

    public Helmet helmet;
    public Upper upper;
    public Lower lower;
    public RightHand rightHand;
    public LeftHand leftHand;

    public int gold;


    private void Start()
    {
        pos = transform.position;
        meleeAttackButton.onClick.AddListener(WaitForAttackInput);
        magicAttackButton.onClick.AddListener(MagicPressed);
        potionButton.onClick.AddListener(UseItem);

        
        target = GameObject.Find("Enemy").GetComponent<CharacterEntity>();

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

    private void WaitForAttackInput()
    {
        initialPos = transform.position;
        targetReached = false;
        attacking = true;
    }

    private void MagicPressed()
    {
        attacking = true;
        RangedAttack();

    }

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
        CheckForLevelUp();
    }

    public void CheckForLevelUp()
    {
        while(exp >= expThreshold)
        {
            float extraXP = exp - expThreshold;
            LevelUp();

            infoText.text = "Level up!";
            Instantiate(infoText, transform.position, Quaternion.identity);

            exp += extraXP;
        }

    }

    // -----------------------------------------------------------------------------

    public void InitializeInventory()
    {
        itemList.Clear();
    }

    public void AddItem(Item item)
    {
        // For item getting added from some source
        int index = LookForFreeInventorySpace();

        itemList[index] = item;
        invDisplay.AddItemImage(item,index);
    }

    public void AddItem(Item item, int index)
    {
        // For item slot switching
        itemList[index] = item;
        invDisplay.AddItemImage(item,index);
    }

    public void RemoveItem(int index)
    {
        itemList[index] = null;
        invDisplay.RemoveItemImage(index);
    }

    public void EquipItem(Item item, int index)
    {
        // Equipping without anything equipped
        if(item is ArmorItem)
        {
            EquipArmorItem((ArmorItem)item);
        }
        else if(item is WeaponItem)
        {
            EquipWeaponItem((WeaponItem)item);
        }
        UpdateDamageStats();
        RemoveItem(index);
    }

    public void EquipItem(Item item)
    {
        // Swapping an equipped item
        if(item is ArmorItem)
        {
            EquipArmorItem((ArmorItem)item);
        }
        else if(item is WeaponItem)
        {
            EquipWeaponItem((WeaponItem)item);
        }
        UpdateDamageStats();

    }

    public void EquipArmorItem(ArmorItem item)
    {
        if(item is Helmet)
        {
            helmet = (Helmet)item;
            charPanel.AddItem(helmet, charPanel.helmetNumber);
        }
        else if(item is Upper)
        {
            upper = (Upper)item;
            charPanel.AddItem(upper, charPanel.upperNumber);
        }
        else if(item is Lower)
        {
            lower = (Lower)item;
            charPanel.AddItem(lower, charPanel.lowerNumber);
        }
        else if(item is LeftHand)
        {
            leftHand = (LeftHand)item;
            charPanel.AddItem(leftHand, charPanel.leftNumber);
        }

        def += item.defense;
    }

    public void EquipWeaponItem(WeaponItem item)
    {
        rightHand = (RightHand)item;
        charPanel.AddItem(rightHand, charPanel.rightNumber);
        minDamage += item.minDamage;
        maxDamage += item.maxDamage;
    }
    
    public void UnequipItem(EquippableItem item)
    {
        // For unequipping only
        if(item is ArmorItem)
        {
            UnequipArmorItem((ArmorItem)item);
        }
        else if(item is WeaponItem)
        {
            UnequipWeaponItem((WeaponItem)item);
        }

        int index = LookForFreeInventorySpace();
        
        itemList[index] = item;
        invDisplay.AddItemImage(item,index);

    }

    public void UnequipItem(EquippableItem item, int index)
    {
        // For swapping equips
        if(item is ArmorItem)
        {
            UnequipArmorItem((ArmorItem)item);
        }
        else if(item is WeaponItem)
        {
            UnequipWeaponItem((WeaponItem)item);
        }

        itemList[index] = item;
        invDisplay.AddItemImage(item,index);
    }

    public void UnequipItemAndDrop(EquippableItem item)
    {
        // Dropping the item out of the bounds of the UI
        if(item is ArmorItem)
        {
            UnequipArmorItem((ArmorItem)item);
        }
        else if(item is WeaponItem)
        {
            UnequipWeaponItem((WeaponItem)item);
        }

        item = null;
    }

    public void UnequipArmorItem(ArmorItem item)
    {
        if(item is Helmet)
        {
            helmet = null;
        }
        else if(item is Upper)
        {
            upper = null;
        }
        else if(item is Lower)
        {
            lower = null;
        }
        else if(item is LeftHand)
        {
            leftHand = null;
        }
        def -= item.defense;
    }

    public void UnequipWeaponItem(WeaponItem item)
    {
        rightHand = null;
        str -= item.minDamage;
        str -= item.maxDamage;
    }

    // -----------------------------------------------------------------------------

    public void SwapItem(int indexA, int indexB)
    {
        Item temp = itemList[indexA];
        itemList[indexA] = itemList[indexB];
        itemList[indexB] = temp;
        invDisplay.SwapItem(indexA,indexB);
    }

    public void GainMoney(int amount)
    {
        gold += amount;
        invDisplay.goldText.text = gold.ToString();
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
