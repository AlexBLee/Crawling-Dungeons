using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Player player;

    public Button attackButton;
    public Button magicButton;
    public Button itemButton;

    public Button magicBackButton;
    public Button potionBackButton;

    public Button pauseButton;
    public GameObject pauseScreen;

    public GameObject gameOverPanel;
    public GameObject winPanel;

    public MagicHUD MagicHUD;
    public PotionHUD PotionHUD;
    public VictoryPanelHUD VictoryPanelHUD;
    public StatusHUD StatusHUD;

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

    public void ShowVictoryPanel()
    {
        VictoryPanelHUD.Show();
    }

    public void HideVictoryPanel()
    {
        VictoryPanelHUD.Hide();
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
            HideVictoryPanel();
            winPanel.SetActive(true);
        }
    }

    public void ActivatePause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0.0f;
    }
}
