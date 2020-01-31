#pragma warning disable CS0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class ShopDisplay : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private List<GameObject> pages;
    [SerializeField] private int currentPage;

    [SerializeField] private List<Item> itemList;
    [SerializeField] private List<ItemDisplay> itemDisplays;

    [SerializeField] private Button shopButton, exitButton, switchPageForwardButton, switchPageBackwardsButton;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private GameObject shop, inventory, itemPopup;

    void Start()
    {
        player.inventory.shopDisplay = this;

        // GameObject.Find will not find ItemPopup as it is initially disabled so have to use this funky work around
        itemPopup = GameObject.Find("Pivot").transform.Find("ItemPopup").gameObject;

        shopButton.onClick.AddListener(ShowShop);
        exitButton.onClick.AddListener(ExitShop);
        switchPageForwardButton.onClick.AddListener(SwitchPageForward);
        switchPageBackwardsButton.onClick.AddListener(SwitchPageBackwards);


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

    private void SwitchPageForward()
    {
        if(pages[currentPage].activeSelf && pages[currentPage + 1] != null)
        {
            pages[currentPage].SetActive(false);
            pages[currentPage + 1].SetActive(true);
            currentPage++;
        }

        if(currentPage == pages.Count - 1)
        {
            switchPageForwardButton.gameObject.SetActive(false);
            switchPageBackwardsButton.gameObject.SetActive(true);

        }
    }

    private void SwitchPageBackwards()
    {
        if(pages[currentPage].activeSelf && pages[currentPage - 1] != null)
        {
            pages[currentPage].SetActive(false);
            pages[currentPage - 1].SetActive(true);
            currentPage--;
        }

        if(currentPage == 0)
        {
            switchPageBackwardsButton.gameObject.SetActive(false);
            switchPageForwardButton.gameObject.SetActive(true);

        }
    }
}
