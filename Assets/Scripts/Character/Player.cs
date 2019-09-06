using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;


public class Player : CharacterEntity
{
    private string TAG_ENEMY = "Enemy";
    private float maxDistance = 1000.0f;
    private float clickedDistance = 1.0f;
    private Rigidbody rb;
    public Vector3 pos;
    private Vector3 enemyPosition;
    public Button btn;
    private bool buttonPressed;
    private InventoryDisplay invDisplay;
    private CharacterPanel charPanel;
    private StatDisplayer statDisplayer;
    private AttackHeader attackHeader;

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
        rb = GetComponent<Rigidbody>();
        pos = transform.position;
        invDisplay = FindObjectOfType<InventoryDisplay>();
        charPanel = FindObjectOfType<CharacterPanel>();
        anim = GetComponent<Animator>();

        
        if(inBattle)
        {
            btn = GameObject.Find("AttackButton").GetComponent<Button>();
            btn.onClick.AddListener(WaitForAttackInput);
        }
    }

    private void Update()
    {
        // --------------------------------------------------------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------------------------------------------------------- //

        if(!inBattle)
        {
            // Test input.
            if(Input.GetKeyDown(KeyCode.F))
            {
                AddItem(tempItem);
            }

            if (Input.GetButton("Fire1"))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, maxDistance))
                {
                    if(!EventSystem.current.IsPointerOverGameObject())
                    {
                        pos = hit.point;
                        transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
                    }
                }
            }


            if (Vector3.Distance(transform.position, pos) > clickedDistance)
            {
                anim.SetBool("Run", true); 
                transform.position += transform.forward * Time.deltaTime * moveSpeed;
            }
            else
            {
                anim.SetBool("Run", false);
            }
        }

        // --------------------------------------------------------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------------------------------------------------------- //


        if(inBattle && GameManager.instance.battleManager.playerTurn)
        {
            if(hp <= 0)
            {
                Debug.Log("oof!");
            }

            if (Input.GetButton("Fire1"))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                if (Physics.Raycast(ray, out hit, 1000.0f))
                {
                    if(hit.collider.gameObject.GetComponent<Enemy>() != null)
                    {
                        target = hit.collider.gameObject.GetComponent<CharacterEntity>();
                        enemyPosition = hit.collider.gameObject.transform.position;
                        targetChosen = true;
                    }
                }
            }

            
            MoveAndAttack(enemyPosition, 1);
            StopAttacking();            
        }

        // --------------------------------------------------------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------------------------------------------------------- //

    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == TAG_ENEMY)
        {
            ApplyStatsTo(GameManager.instance.playerStats);
            CopyInventoryTo(GameManager.instance.itemList);

            GameManager.instance.lastCameraPos = Camera.main.transform.position;
            GameManager.instance.lastPlayerPos = transform.position;
            SceneManager.LoadScene("FightScene");
        }


    }

    // -----------------------------------------------------------------------------

    private void WaitForAttackInput()
    {
        if(targetChosen)
        {
            GameManager.instance.battleManager.attackHeader.gameObject.SetActive(true);
            GameManager.instance.battleManager.attackHeader.UpdateText("Attack");
            initialPos = transform.position;
            targetReached = false;
            attacking = true;
        }
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
            GameManager.instance.playerLevel = level;
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

    public void CopyInventoryTo(List<Item> playerInv)
    {
        for(int i = 0; i < 3; i++)
        {
            playerInv[i] = itemList[i];
        }
    }

    public void CopyInventoryFrom(List<Item> playerInv)
    {
        for(int i = 0; i < itemList.Count; i++)
        {
            itemList[i] = playerInv[i];
        }
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
