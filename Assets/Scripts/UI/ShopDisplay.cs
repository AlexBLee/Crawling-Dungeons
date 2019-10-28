using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShopDisplay : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public Player player;
    public List<Item> itemList;
    public List<ItemDisplay> itemDisplays;
    public Button shopButton;
    public Button exitButton;
    public GameObject shop;
    public GameObject inventory;

    void Start()
    {
        shopButton.onClick.AddListener(ShowShop);
        exitButton.onClick.AddListener(ExitShop);

        if(moneyText != null)
        {
            UpdateGold();
        }


        for(int i = 0; i < itemList.Count; i++)
        {
            itemDisplays[i].nameOfItem.text = itemList[i].itemName;
            itemDisplays[i].cost.text = itemList[i].cost.ToString();
            itemDisplays[i].image.sprite = itemList[i].image;
        }

        // Adding listeners for the buttons does not work with loops due to a Unity bug.
        // Manually writing them out is currently the only solution.
        itemDisplays[0].button.onClick.AddListener(delegate {player.AddItem(itemList[0]);});
        itemDisplays[1].button.onClick.AddListener(delegate {player.AddItem(itemList[1]);});
        itemDisplays[2].button.onClick.AddListener(delegate {player.AddItem(itemList[2]);});
        itemDisplays[3].button.onClick.AddListener(delegate {player.AddItem(itemList[3]);});
        itemDisplays[4].button.onClick.AddListener(delegate {player.AddItem(itemList[4]);});
        itemDisplays[5].button.onClick.AddListener(delegate {player.AddItem(itemList[5]);});
        itemDisplays[6].button.onClick.AddListener(delegate {player.AddItem(itemList[6]);});
        itemDisplays[7].button.onClick.AddListener(delegate {player.AddItem(itemList[7]);});
        itemDisplays[8].button.onClick.AddListener(delegate {player.AddItem(itemList[8]);});
        
    }

    public void ShowShop()
    {
        inventory.SetActive(false);
        shop.SetActive(true);
    }

    public void UpdateGold()
    {
        moneyText.text = player.gold.ToString();
    }

    public void ExitShop()
    {

    }
}
