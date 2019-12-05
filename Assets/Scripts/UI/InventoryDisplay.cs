using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryDisplay : MonoBehaviour
{
    // Itemslots display
    public List<Item> items;
    public List<Image> images;
    public List<Button> buttons;

    // Equipped items display
    public List<EquippableItem> equippedItems;
    public List<Image> equippedImages;
    public List<Button> equippedButtons;

    public Sprite blankImage;
    public Sprite blankEquipImage;

    public Player player;
    public int count = 0;
    private int tempIndex;
    private bool notFront;
    public ItemPopup itemPopup;
    public TextMeshProUGUI goldText;
    public Button inventoryButton;
    public GameObject inventory;
    public GameObject shop;


    private void Start() 
    {
        items = player.itemList;
        inventoryButton.onClick.AddListener(ShowInventory);

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

        goldText.text = player.gold.ToString();
    }

    public void ShowInventory()
    {
        AudioManager.Instance.Play("Bag");
        shop.SetActive(false);
        inventory.SetActive(true);
    }

    public void AddItemImage(Item item, int index)
    {
        items[index] = item;
        images[index].sprite = items[index].image;

        if(notFront)
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
        equippedImages[index].sprite = item.image;
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

    public void DisplayItemInfo(int index, Vector3 position)
    {
        if(items[index] != null)
        {
            itemPopup.gameObject.SetActive(true);
            
            if(items[index] is ConsumableItem)
            {
                itemPopup.equipButton.gameObject.SetActive(false);
                itemPopup.unequipButton.gameObject.SetActive(false);
            }
            else
            {
                itemPopup.equipButton.gameObject.SetActive(true);
                itemPopup.unequipButton.gameObject.SetActive(false);
            }

            itemPopup.nameOfItem.text = items[index].itemName;
            itemPopup.description.text = items[index].description;
            itemPopup.transform.parent.position = position;

            itemPopup.item = items[index];
            itemPopup.index = index;

            if(items[index] is EquippableItem)
            {
                EquippableItem item = (EquippableItem)items[index];
                itemPopup.equipIndex = item.itemType;
            }

        }
    }

    public void DisplayEquippedItemInfo(int index, Vector3 position)
    {
        if(equippedItems[index] != null)
        {
            itemPopup.gameObject.SetActive(true);
            itemPopup.unequipButton.gameObject.SetActive(true);
            itemPopup.equipButton.gameObject.SetActive(false);
            itemPopup.trashButton.gameObject.SetActive(false);

            itemPopup.nameOfItem.text = equippedItems[index].itemName;
            itemPopup.description.text = equippedItems[index].description;
            itemPopup.transform.parent.position = position;

            itemPopup.item = equippedItems[index];
            itemPopup.index = index;

        }
    }


    


}
