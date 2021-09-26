#pragma warning disable CS0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryDisplay : MonoBehaviour
{
    // Item slots display
    public List<Item> items;

    [SerializeField] private List<Image> images;
    [SerializeField] private List<Button> buttons;

    // Equipped items display
    public List<EquippableItem> equippedItems;

    [SerializeField] private List<Image> equippedImages;
    [SerializeField] private List<Button> equippedButtons;
    [SerializeField] private Sprite blankImage, blankEquipImage;
    
    [SerializeField] private Player player;

    private int count = 0, tempIndex;
    private bool notFront;
    
    [SerializeField] private ItemPopup itemPopup;
    public TextMeshProUGUI goldText;
    public Button inventoryButton;

    [SerializeField] private GameObject inventory, shop;


    private void Start() 
    {
        player.inventory.inventoryDisplay = this;
        // player.inventory.ApplyItemsFrom(GameManager.instance.playerStats);

        items = player.inventory.items;
        equippedItems = player.inventory.equips;

        inventoryButton.onClick.AddListener(ShowInventory);

        // player.inventory.UpdateItemStats();

        for(int i = 0; i < buttons.Count; i++)
        {
            int x = i;
            buttons[x].onClick.AddListener(delegate {DisplayItemInfo(x,buttons[x].gameObject.transform.position);});
        }

        for(int i = 0; i < equippedButtons.Count; i++)
        {
            int x = i;
            equippedButtons[x].onClick.AddListener(delegate {DisplayEquippedItemInfo(x,equippedButtons[x].gameObject.transform.position);});
        }

        UpdateInventory();
        goldText.text = player.inventory.gold.ToString();
    }

    private void ShowInventory()
    {
        itemPopup.gameObject.SetActive(false);
        AudioManager.Instance.PlaySound("Bag");
        shop.SetActive(false);
        inventory.SetActive(true);
    }

    private void UpdateInventory()
    {
        // Updating item inventory
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
            {
                images[i].sprite = GameDatabase.instance.GetSprite(items[i].itemName);
            }
        }

        // Updating equip inventory
        for (int i = 0; i < equippedItems.Count; i++)
        {
            if (equippedItems[i] != null)
            {
                equippedImages[i].sprite = GameDatabase.instance.GetSprite(equippedItems[i].itemName);
            }
        }
    }

    public void AddItemImage(Item item, int index)
    {
        items[index] = item;
        images[index].sprite = GameDatabase.instance.GetSprite(item.itemName);;

        if (notFront)
        {
            count = tempIndex;
        }
        else
        {
            count++;
        }
        notFront = false;
    }

    public void AddEquippedItemImage(EquippableItem item, int index)
    {
        equippedItems[index] = item;
        equippedImages[index].sprite = GameDatabase.instance.GetSprite(item.itemName);
    }

    public void RemoveItemImage(int index)
    {
        images[index].sprite = blankImage;
        tempIndex = count;
        
        count = index;
        notFront = true;
    }

    public void RemoveEquippedItemImage(int index)
    {
        equippedImages[index].sprite = blankEquipImage;
        tempIndex = count;
        
        count = index;
        notFront = true;
    }

    public void SwapItem(int indexA, int indexB)
    {
        Sprite imgTemp = images[indexA].sprite;
        images[indexA].sprite = images[indexB].sprite;
        images[indexB].sprite = imgTemp;
    }

    private void DisplayItemInfo(int index, Vector3 position)
    {
        if(items[index] != null)
        {
            itemPopup.gameObject.SetActive(true);
            
            if(items[index] is ConsumableItem)
            {
                itemPopup.DisableAllButtons();
                itemPopup.sellButton.gameObject.SetActive(true);
            }
            else
            {
                itemPopup.DisableAllButtons();
                itemPopup.equipButton.gameObject.SetActive(true);
                itemPopup.sellButton.gameObject.SetActive(true);
            }

            itemPopup.nameOfItem.text = items[index].itemName;
            itemPopup.description.text = items[index].description.Replace("\\n", "\n");
            itemPopup.transform.parent.position = position;

            RectTransform rct = itemPopup.transform.GetComponent<RectTransform>();
            rct.offsetMin = new Vector2(0, rct.offsetMin.y);
            rct.offsetMax = new Vector2(0, rct.offsetMin.y);

            itemPopup.item = items[index];
            itemPopup.index = index;

            if(items[index] is EquippableItem)
            {
                EquippableItem item = (EquippableItem)items[index];
                itemPopup.equipIndex = item.itemType;
            }



        }
    }

    private void DisplayEquippedItemInfo(int index, Vector3 position)
    {
        if(equippedItems[index] != null)
        {
            itemPopup.DisableAllButtons();
            itemPopup.gameObject.SetActive(true);
            itemPopup.unequipButton.gameObject.SetActive(true);

            itemPopup.nameOfItem.text = equippedItems[index].itemName;
            itemPopup.description.text = equippedItems[index].description.Replace("\\n", "\n");

            itemPopup.item = equippedItems[index];
            itemPopup.transform.parent.position = position;
            itemPopup.index = index;

            // For position the UI correctly so everything fits on screen.
            // // For left hand slot
            if(index == 3)
            {
                RectTransform rct = itemPopup.transform.GetComponent<RectTransform>();
                rct.offsetMin = new Vector2(Screen.width * 0.400f, rct.offsetMin.y);
                rct.offsetMax = new Vector2(Screen.width * 0.400f, rct.offsetMin.y);
            }

        }
    }

    public void UpdateItemAmount(int index)
    {
        if(items[index] == null)
        {
            images[index].transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            images[index].transform.GetChild(0).gameObject.SetActive(true);
            images[index].GetComponentInChildren<TextMeshProUGUI>().text = items[index].amount.ToString();
        }
    }
    


}
