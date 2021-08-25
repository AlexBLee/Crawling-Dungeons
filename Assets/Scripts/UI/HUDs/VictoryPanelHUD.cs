using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryPanelHUD : HUDMenu
{
    [SerializeField] private List<AddRemoveStat> addRemoves;
    [SerializeField] private TextMeshProUGUI levelNumber;

    public override void Show()
    {
        base.Show();
        levelNumber.text = player.level.ToString();
    }

    private void DeactivateSubtractors()
    {
        for (int i = 0; i < addRemoves.Count; i++)
        {
            addRemoves[i].DeactivateSubtractor();
        }
    }

    private void ActivateSubtractorsIfModified()
    {
        for (int i = 0; i < addRemoves.Count; i++)
        {
            addRemoves[i].ActivateSubtractorIfModified();
        }
    }

    private void DeactivateAdders()
    {
        for (int i = 0; i < addRemoves.Count; i++)
        {
            addRemoves[i].DeactivateAdder();    
        }
    }

    private void ActivateAdders()
    {
        for (int i = 0; i < addRemoves.Count; i++)
        {
            addRemoves[i].ActivateAdder();
        }
    }

    public void ActivateAddersOnly()
    {
        DeactivateSubtractors();
        ActivateAdders();
    }

    public void ActivateModifiedSubtractors()
    {
        DeactivateAdders();
        ActivateSubtractorsIfModified();
    }

    public void ActivateAllStatModifiers()
    {
        ActivateSubtractorsIfModified();
        ActivateAdders();
    }
}
