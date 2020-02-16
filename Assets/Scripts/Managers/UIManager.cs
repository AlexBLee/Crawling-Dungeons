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

    public TextMeshProUGUI levelNumber;

    public GameObject victoryPanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    private const int itemMaxLocationCount = 24; // need an alternative for this number
    private const float buttonFadeAmount = 0.5f;

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

        foreach(Button b in potionButtonList)
        {
            int x = count;
            if(player.inventory.consumableLocations[x] < itemMaxLocationCount)
            {
                b.onClick.AddListener(delegate{player.inventory.ConsumeItem(player.inventory.consumableLocations[x], player);});
            }
            count++;
        }

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

                if(player.mp < spells.Key.cost)
                {
                    FadeButtons(magicButtonList[counter], magicButtonList[counter].GetComponentInChildren<TextMeshProUGUI>());
                }
                else
                {
                    RestoreButtonDefaults(magicButtonList[counter], magicButtonList[counter].GetComponentInChildren<TextMeshProUGUI>());
                }        
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
            if(player.inventory.consumableLocations[x] < itemMaxLocationCount && player.inventory.items[player.inventory.consumableLocations[x]] != null)
            {
                potionTextList[x].text = 
                player.inventory.items[player.inventory.consumableLocations[x]].itemName +
                " x" + player.inventory.items[player.inventory.consumableLocations[x]].amount.ToString();
            }
            // If the potion no longer exists..
            else
            {
                FadeButtons(b, potionTextList[x], "N/A");
            }
            count++;
        }
    }
    
    public void DeactivateSubtractors()
    {
        for(int i = 0; i < addRemoves.Count; i++)
        {
            if(!addRemoves[i].modified)
            {
                addRemoves[i].minus.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateSubtractors()
    {
        for(int i = 0; i < addRemoves.Count; i++)
        {
            if(addRemoves[i].modified)
            {
                addRemoves[i].minus.gameObject.SetActive(true);
            }
            else
            {
                addRemoves[i].minus.gameObject.SetActive(false);
            }
        }
    }

    public void DeactivateAdders()
    {
        for(int i = 0; i < addRemoves.Count; i++)
        {
            addRemoves[i].plus.gameObject.SetActive(false);      
        }
    }

    public void ActivateAdders()
    {
        for(int i = 0; i < addRemoves.Count; i++)
        {
            addRemoves[i].plus.gameObject.SetActive(true);
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

    // For magic list
    private void FadeButtons(Button button, TextMeshProUGUI text)
    {
        // Keep the colour faded and disable the button.
        Color tempColor = text.color;

        tempColor.a = buttonFadeAmount;
        text.color = tempColor;
        
        button.interactable = false;
    }

    // For potion list
    private void FadeButtons(Button button, TextMeshProUGUI text, string message)
    {
        // Keep the colour faded and disable the button.
        Color tempColor = text.color;

        tempColor.a = buttonFadeAmount;
        text.color = tempColor;

        text.text = message;
        
        button.interactable = false;
    }

    private void RestoreButtonDefaults(Button button, TextMeshProUGUI text)
    {
        Color tempColor = text.color;

        tempColor.a = 1.0f;
        text.color = tempColor;

        button.interactable = true;
    }

    public void ActivateGameOver()
    {
        DisableButtons();
        gameOverPanel.SetActive(true);
    }

    public void ActivateGameWin()
    {
        if(winPanel != null)
        {
            DisableButtons();
            victoryPanel.SetActive(false);
            winPanel.SetActive(true);
        }
    }

}
