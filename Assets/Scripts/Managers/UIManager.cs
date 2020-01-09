using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Player player;

    public Button attackButton;
    public Button magicButton;
    public Button itemButton;

    public GameObject magicList;
    public List<Button> magicButtonList;

    public GameObject potionList;
    public List<Button> potionButtonList;
    public List<TextMeshProUGUI> potionTextList;

    public List<AddRemoveStat> addRemoves;

    public AmountBar healthBar;
    public AmountBar manaBar;

    public Button magicBackButton;
    public Button potionBackButton;

    public GameObject victoryPanel;
    public GameObject gameOverPanel;

    private void Start() 
    {
        attackButton.onClick.AddListener(player.Attack);
        magicButton.onClick.AddListener(ShowMagicList);
        itemButton.onClick.AddListener(ShowPotionList);

        int count = 0;
        foreach(KeyValuePair<Spell, bool> spell in player.spells.spellList)
        {
            magicButtonList[count].onClick.AddListener(delegate{player.MagicPressed(spell.Key);});
            count++;
        }
        count = 0;

        DisplayPotionAmount();

        magicBackButton.onClick.AddListener(HideMagicList);
        potionBackButton.onClick.AddListener(HidePotionList);

    }

    public void DisableButtons()
    {
        attackButton.gameObject.SetActive(false);
        magicButton.gameObject.SetActive(false);
        itemButton.gameObject.SetActive(false);
    }

    public void EnableButtons()
    {
        attackButton.gameObject.SetActive(true);
        magicButton.gameObject.SetActive(true);
        itemButton.gameObject.SetActive(true);
    }

    public void ShowMagicList()
    {
        DisableButtons();
        magicList.SetActive(true);
        int counter = 0;

        // Show only unlocked spells
        foreach(KeyValuePair<Spell,bool> spells in player.spells.spellList)
        {
            if(!spells.Value)
            {
                magicButtonList[counter].gameObject.SetActive(false);
            }
            else
            {
                magicButtonList[counter].gameObject.SetActive(true);                
            }
            counter++;
        }

    }

    public void HideMagicList()
    {
        EnableButtons();
        magicList.SetActive(false);
    }

    public void ShowPotionList()
    {
        DisableButtons();
        potionList.SetActive(true);
        DisplayPotionAmount();
    }

    public void HidePotionList()
    {
        EnableButtons();
        potionList.SetActive(false);
    }

    private void DisplayPotionAmount()
    {
        int count = 0;
        foreach(Button b in potionButtonList)
        {
            int x = count;
            if(player.inventory.consumableLocations.Count > 0 && player.inventory.items[player.inventory.consumableLocations[x]] != null)
            {
                potionTextList[x].text = 
                player.inventory.items[player.inventory.consumableLocations[x]].itemName +
                " x" + player.inventory.items[player.inventory.consumableLocations[x]].amount.ToString();
            }
            // If the potion no longer exists..
            else
            {
                // Keep the colour faded and disable the button.
                Image img = b.GetComponent<Image>();
                Color tempColor = img.color;

                tempColor.a = 170f;
                img.color = tempColor;

                tempColor = potionTextList[x].color;
                tempColor.a = 0.5f;
                potionTextList[x].color = tempColor;
                
                potionTextList[x].text = "N/A";

                b.interactable = false;
            }
            count++;
        }
    }

    public void DeactivateAdders()
    {
        for(int i = 0; i < addRemoves.Count; i++)
        {
            addRemoves[i].gameObject.SetActive(false);
        }
    }

    public void ActivateAdders()
    {
        for(int i = 0; i < addRemoves.Count; i++)
        {
            addRemoves[i].gameObject.SetActive(true);
        }
    }

    public void UpdateUIHealth()
    {
        healthBar.SetAmount(player.hp, player.maxHP);
    }

    public void UpdateUIMana()
    {
        manaBar.SetAmount(player.mp, player.maxMP);
    }

    public void ActivateGameOver()
    {
        DisableButtons();
        gameOverPanel.SetActive(true);
    }

}
