﻿#pragma warning disable CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPopup : MonoBehaviour
{
    
    [SerializeField] private Player player;
    
    public Item item;
    public int index;
    public int equipIndex;

    public TextMeshProUGUI nameOfItem;
    public TextMeshProUGUI description;

    public Button equipButton;
    public Button trashButton;
    public Button unequipButton;
    public Button sellButton;
    

    private void Start() 
    {
        player.inventory.itemPopup = this;

        equipButton.onClick.AddListener(delegate{player.inventory.EquipItem(index, equipIndex);});
        trashButton.onClick.AddListener(delegate{player.inventory.RemoveItem(index, false);});

        unequipButton.onClick.AddListener(delegate{player.inventory.UnequipItem(index);});
        sellButton.onClick.AddListener(delegate{player.inventory.SellItem(index);});

    }
}
