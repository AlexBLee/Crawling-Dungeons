﻿using System.Collections.Generic;
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
    public List<PotionUI> potionUIList;

    public List<AddRemoveStat> addRemoves;

    public AmountBar healthBar;
    public AmountBar manaBar;

    public Button magicBackButton;
    public Button potionBackButton;

    public TextMeshProUGUI levelNumber;

    public Button pauseButton;
    public GameObject pauseScreen;

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

        for (int i = 0; i < player.spells.spells.Count; i++)
        {
            SpellNew spell = player.spells.spells[i];
            magicButtonList[i].onClick.AddListener(() => { player.MagicPressed(spell); });
        }

        // reset the potions so the buttons can re-link to the potions in the new scene.
        foreach (ConsumableItem potion in player.inventory.items)
        {
            if (potion != null)
            {
                potion.marked = false;
            }
        }

        foreach(PotionUI potionUI in potionUIList)
        {
            foreach (ConsumableItem potion in player.inventory.items)
            {
                if(potion != null && !potion.marked)
                {
                    potion.marked = true;
                    potionUI.button.onClick.AddListener(delegate{player.inventory.ConsumeItem(player.inventory.items.IndexOf(potion), player);});
                    potionUI.item = potion;
                    break;
                }
            }
        }

        DisplayPotionAmount();

        magicBackButton.onClick.AddListener(HideMagicList);
        potionBackButton.onClick.AddListener(HidePotionList);
        pauseButton.onClick.AddListener(ActivatePause);

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
        foreach(SpellNew spell in player.spells.spells)
        {
            if(!spell.unlocked)
            {
                magicButtonList[counter].gameObject.SetActive(false);
            }
            else
            {
                magicButtonList[counter].gameObject.SetActive(true);

                if(player.mp < spell.Cost)
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
        foreach (PotionUI potionUI in potionUIList)
        {
            if(potionUI.item != null)
            {
                if (potionUI.item.amount == 0)
                {
                    potionUI.item = null;
                    FadeButtons(potionUI.button, potionUI.text, "N/A");
                }
                else
                {
                    potionUI.text.text = potionUI.item.itemName + " x" + potionUI.item.amount.ToString();
                }
            }
            else
            {
                FadeButtons(potionUI.button, potionUI.text, "N/A");
            }
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

    public void ActivatePause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0.0f;
    }
}
