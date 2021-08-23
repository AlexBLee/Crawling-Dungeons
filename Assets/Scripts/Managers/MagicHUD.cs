using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MagicHUD : HUDMenu
{
    public List<Button> magicButtonList;

    public override void Init()
    {
        RefreshMagicList();
    }

    public override void Show()
    {
        gameObject.SetActive(true);
        RefreshMagicList();
        CheckMagicButtonInteractivity();
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    private void RefreshMagicList()
    {
        RefreshMagicButtons();

        List<Spell> spellList = player.spells.GetUnlockedSpells();

        for (int i = 0; i < spellList.Count; i++)
        {
            Spell spell = spellList[i];
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
        List<Spell> spellList = player.spells.GetUnlockedSpells();

        for (int i = 0; i < spellList.Count; i++)
        {
            Spell spell = spellList[i];
            Button magicButton = magicButtonList[i];

            magicButton.interactable = player.HasEnoughManaForSpell(spell) ? true : false;
        }
    }
}
