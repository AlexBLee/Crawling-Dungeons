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

    void Start()
    {
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

        itemDisplays[0].button.onClick.AddListener(delegate {player.AddItem(itemList[0]);});
        itemDisplays[1].button.onClick.AddListener(delegate {player.AddItem(itemList[1]);});
        itemDisplays[2].button.onClick.AddListener(delegate {player.AddItem(itemList[2]);});




        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGold()
    {
        moneyText.text = player.gold.ToString();
    }
}
