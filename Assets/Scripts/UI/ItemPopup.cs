using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPopup : MonoBehaviour
{
    public Player player;
    public Item item;
    public int index;
    public int equipIndex;

    public TextMeshProUGUI nameOfItem;
    public TextMeshProUGUI description;

    public Button equipButton;
    public Button trashButton;

    public Button unequipButton;
    

    private void Start() 
    {
        equipButton.onClick.AddListener(delegate{player.inventory.EquipItem(index, equipIndex);});
        trashButton.onClick.AddListener(delegate{player.inventory.RemoveItem(index);});

        unequipButton.onClick.AddListener(delegate{player.inventory.UnequipItem(index);});



    }

}
