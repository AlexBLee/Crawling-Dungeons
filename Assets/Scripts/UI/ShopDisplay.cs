using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class ShopDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI moneyText;
    
    [SerializeField]
    private Player player;

    [SerializeField]
    private List<Item> itemList;

    [SerializeField]
    private List<ItemDisplay> itemDisplays;

    [SerializeField]
    private Button shopButton, exitButton;

    [SerializeField]
    private GameObject shop, inventory, itemPopup;

    void Start()
    {
        player.inventory.shopDisplay = this;

        // GameObject.Find will not find ItemPopup as it is initially disabled so have to use this funky work around
        itemPopup = GameObject.Find("Pivot").transform.Find("ItemPopup").gameObject;

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

        for(int i = 0; i < itemDisplays.Count; i++)
        {
            int x = i;
            itemDisplays[x].button.onClick.AddListener(delegate {player.inventory.BuyItem(itemList[x]);});
        }
        
    }

    private void ShowShop()
    {
        if(itemPopup.activeSelf)
        {
            itemPopup.SetActive(false);
        }
        inventory.SetActive(false);
        shop.SetActive(true);
    }

    public void UpdateGold()
    {
        moneyText.text = player.inventory.gold.ToString();
    }

    private void ExitShop()
    {
        player.inventory.ApplyItemsTo(GameManager.instance.playerStats);
        SceneManager.LoadScene(GameManager.instance.levelNumber);
    }
}
