using System.Collections.Generic;
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

    public ConsumableItem healthPot;
    public ConsumableItem manaPot;

    public List<AddRemoveStat> addRemoves;

    public AmountBar healthBar;
    public AmountBar manaBar;

    public Button magicBackButton;
    public Button potionBackButton;

    public GameObject gameOverPanel;

    private void Start() 
    {
        attackButton.onClick.AddListener(player.Attack);
        magicButton.onClick.AddListener(ShowMagicList);
        itemButton.onClick.AddListener(ShowPotionList);

        int count = 0;
        foreach(KeyValuePair<Spell, bool> spell in player.spellList)
        {
            magicButtonList[count].onClick.AddListener(delegate{player.MagicPressed(spell.Key);});
            count++;
        }

        foreach(ConsumableItem item in player.itemList)
        {
            if(item == healthPot)
            {
                player.hpCounter++;
            }
            else if(item == manaPot)
            {
                player.mpCounter++;
            }
        }

        potionButtonList[0].onClick.AddListener(delegate{player.UseHealthItem(healthPot);});
        potionButtonList[1].onClick.AddListener(delegate{player.UseManaItem(manaPot);});

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

        foreach(KeyValuePair<Spell,bool> spells in player.spellList)
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
        
        potionTextList[0].text = "x" + player.hpCounter;
        potionTextList[1].text = "x" + player.mpCounter;



    }

    public void HidePotionList()
    {
        EnableButtons();
        potionList.SetActive(false);
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
