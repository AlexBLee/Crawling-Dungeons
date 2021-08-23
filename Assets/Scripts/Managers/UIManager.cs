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

    public MagicHUD MagicHUD;
    public PotionHUD PotionHUD;

    private void Start() 
    {
        attackButton.onClick.AddListener(player.Attack);
        magicButton.onClick.AddListener(ShowMagicList);
        itemButton.onClick.AddListener(ShowPotionList);

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
        MagicHUD.Show();
    }

    public void HideMagicList()
    {
        EnableButtons();
        MagicHUD.Hide();
    }

    public void ShowPotionList()
    {
        DisableButtons();
        PotionHUD.Show();
    }

    public void HidePotionList()
    {
        EnableButtons();
        PotionHUD.Hide();
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
