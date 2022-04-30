using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class MagicHUD : HUDMenu
{
    [SerializeField] private Player player;
    public List<Button> magicButtonList;

    public void Start()
    {
        RefreshMagicList();
    }

    public override void Show()
    {
        base.Show();
        RefreshMagicList();
        CheckMagicButtonInteractivity();
    }

    private void RefreshMagicList()
    {
        RefreshMagicButtons();

        List<SpellInfo> spellList = player.spells.GetUnlockedSpells();

        for (int i = 0; i < spellList.Count; i++)
        {
            SpellInfo spell = spellList[i];
            Button magicButton = magicButtonList[i];

            magicButton.gameObject.SetActive(true);
            magicButton.GetComponentInChildren<TextMeshProUGUI>().text = spell.name;
            magicButton.onClick.AddListener(() => { player.MagicPressed(spell); });
        }
    }

    private void RefreshMagicButtons()
    {
        foreach (Button button in magicButtonList)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }
    }

    private void CheckMagicButtonInteractivity()
    {
        List<SpellInfo> spellList = player.spells.GetUnlockedSpells();

        for (int i = 0; i < spellList.Count; i++)
        {
            SpellInfo spell = spellList[i];
            Button magicButton = magicButtonList[i];

            magicButton.interactable = player.HasEnoughManaForSpell(spell) ? true : false;
        }
    }
}
