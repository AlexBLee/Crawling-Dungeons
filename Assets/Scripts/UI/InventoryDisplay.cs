﻿using System.Collections;
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
    public List<Item> equippedItems;
    public List<Image> equippedImages;
    public List<Button> equippedButtons;

    // For determining which position to add the item image to.
    [HideInInspector]
    public int helmetNumber = 0, 
    upperNumber = 1, 
    rightHandNumber = 2, 
    leftHandNumber = 3, 
    lowerNumber = 4, 
    bootNumber = 5;

    public Sprite blankImage;
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

        buttons[0].onClick.AddListener(delegate {DisplayItemInfo(0,buttons[0].gameObject.transform.position);});
        buttons[1].onClick.AddListener(delegate {DisplayItemInfo(1,buttons[1].gameObject.transform.position);});
        buttons[2].onClick.AddListener(delegate {DisplayItemInfo(2,buttons[2].gameObject.transform.position);});
        buttons[3].onClick.AddListener(delegate {DisplayItemInfo(3,buttons[3].gameObject.transform.position);});
        buttons[4].onClick.AddListener(delegate {DisplayItemInfo(4,buttons[4].gameObject.transform.position);});
        buttons[5].onClick.AddListener(delegate {DisplayItemInfo(5,buttons[5].gameObject.transform.position);});
        buttons[6].onClick.AddListener(delegate {DisplayItemInfo(6,buttons[6].gameObject.transform.position);});
        buttons[7].onClick.AddListener(delegate {DisplayItemInfo(7,buttons[7].gameObject.transform.position);});
        buttons[8].onClick.AddListener(delegate {DisplayItemInfo(8,buttons[8].gameObject.transform.position);});


    }

    public void ShowInventory()
    {
        shop.SetActive(false);
        inventory.SetActive(true);
    }

    public void AddItemImage(Item item, int index)
    {
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

    public void AddEquippedItemImage(Item item, int index)
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

    public void SwapItem(int indexA, int indexB)
    {
        Sprite imgTemp = images[indexA].sprite;
        images[indexA].sprite = images[indexB].sprite;
        images[indexB].sprite = imgTemp;
    }

    public void DisplayItemInfo(int index, Vector3 position)
    {
        itemPopup.gameObject.SetActive(true);
        itemPopup.nameOfItem.text = items[index].itemName;
        itemPopup.description.text = items[index].description;
        itemPopup.transform.parent.position = position;

        itemPopup.item = items[index];
        itemPopup.index = index;
    }
    


}