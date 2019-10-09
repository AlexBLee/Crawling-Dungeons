using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Player player;

    public Button attackButton;
    public Button magicButton;
    public Button itemButton;

    public GameObject magicList;
    public List<Button> magicButtonList;

    private void Start() 
    {
        attackButton.onClick.AddListener(player.Attack);
        magicButton.onClick.AddListener(ShowMagicList);
        itemButton.onClick.AddListener(player.UseItem);

        magicButtonList[0].onClick.AddListener(delegate{player.MagicPressed(player.spellList[0]);});
        magicButtonList[1].onClick.AddListener(delegate{player.MagicPressed(player.spellList[1]);});
        magicButtonList[2].onClick.AddListener(delegate{player.MagicPressed(player.spellList[2]);});
        magicButtonList[3].onClick.AddListener(delegate{player.MagicPressed(player.spellList[3]);});
        magicButtonList[4].onClick.AddListener(delegate{player.MagicPressed(player.spellList[4]);});
        magicButtonList[5].onClick.AddListener(delegate{player.MagicPressed(player.spellList[5]);});


        HideMagicList();
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
    }

    public void HideMagicList()
    {
        EnableButtons();
        magicList.SetActive(false);
    }
}
