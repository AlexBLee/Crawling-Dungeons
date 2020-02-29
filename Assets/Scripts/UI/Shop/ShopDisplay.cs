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
    [SerializeField] private GameObject shop, inventory;
    [SerializeField] private ItemPopup itemPopup;
    private RectTransform itemPopupRect;
    
    private const float textFadeAmount = 0.5f;
    private const float fitOnScreenFactor = 0.397f;

    void Start()
    {
        player.inventory.shopDisplay = this;
        itemPopupRect = itemPopup.GetComponent<RectTransform>();

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
            itemDisplays[x].button.onClick.AddListener(delegate {DisplayItemInfo(x, itemDisplays[x].gameObject.transform.position);});
        }
        
    }

    private void ShowShop()
    {
        if(itemPopup.gameObject.activeSelf)
        {
            itemPopup.gameObject.SetActive(false);
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
            itemPopup.gameObject.SetActive(false);
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
            itemPopup.gameObject.SetActive(false);
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

    private void DisplayItemInfo(int index, Vector3 position)
    {
        if(itemList[index] != null)
        {
            itemPopup.gameObject.SetActive(true);
            
            if(itemList[index] is ConsumableItem)
            {
                itemPopup.DisableAllButtons();
                itemPopup.buyButton.gameObject.SetActive(true);
                itemPopup.buyAndEquipButton.gameObject.SetActive(false);
            }
            else
            {
                itemPopup.DisableAllButtons();
                itemPopup.buyButton.gameObject.SetActive(true);
                itemPopup.buyAndEquipButton.gameObject.SetActive(true);
            }

            if(player.inventory.gold < itemList[index].cost)
            {
                DisableShopButton(itemPopup.buyButton);
                DisableShopButton(itemPopup.buyAndEquipButton);
            }
            else
            {
                EnableShopButton(itemPopup.buyButton);
                EnableShopButton(itemPopup.buyAndEquipButton);
            }
            // For position the UI correctly so everything fits on screen.
            if((index >= 0 && index <= 2) || (index >= 9 && index <= 11))
            {
                itemPopupRect.offsetMin = new Vector2(Screen.width * fitOnScreenFactor, itemPopupRect.offsetMin.y);
                itemPopupRect.offsetMax = new Vector2(Screen.width * fitOnScreenFactor, itemPopupRect.offsetMin.y);
            }
            else
            {
                itemPopupRect.offsetMin = new Vector2(0, itemPopupRect.offsetMin.y);
                itemPopupRect.offsetMax = new Vector2(0, itemPopupRect.offsetMin.y);
            }

            itemPopup.nameOfItem.text = itemList[index].itemName;
            itemPopup.description.text = itemList[index].description.Replace("\\n", "\n");
            itemPopup.transform.parent.position = position;

            itemPopup.item = itemList[index];
            itemPopup.index = index;



        }
    }

    // Only for shop buttons due to being the only ones that can have their interactable turned off.
    private void DisableShopButton(Button button)
    {
        TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();

        Color tempColor = text.color;

        tempColor.a = textFadeAmount;
        text.color = tempColor;
        
        button.interactable = false;
    }

    private void EnableShopButton(Button button)
    {
        TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();

        Color tempColor = text.color;

        tempColor.a = 255f;
        text.color = tempColor;
        
        button.interactable = true;
    }
}
