using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryPanelHUD : HUDMenu
{
    public List<AddRemoveStat> addRemoves;
    private int originalNumberOfStatPoints = 0;

    public override void Show()
    {
        base.Show();
        originalNumberOfStatPoints = player.statPoints;
    }

    public void DeactivateSubtractors()
    {
        for (int i = 0; i < addRemoves.Count; i++)
        {
            if (!addRemoves[i].modified)
            {
                addRemoves[i].minus.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateSubtractors()
    {
        for (int i = 0; i < addRemoves.Count; i++)
        {
            if (addRemoves[i].modified)
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
        for (int i = 0; i < addRemoves.Count; i++)
        {
            addRemoves[i].plus.gameObject.SetActive(false);      
        }
    }

    public void ActivateAdders()
    {
        for (int i = 0; i < addRemoves.Count; i++)
        {
            addRemoves[i].plus.gameObject.SetActive(true);
        }
    }

    private void CheckStatAmount()
    {
        if (player.statPoints == 0)
        {
            DeactivateAdders();
            ActivateSubtractors();
        }
        else if (player.statPoints == originalNumberOfStatPoints)
        {
            DeactivateSubtractors();
            ActivateAdders();
        }
        else if (player.statPoints > 0)
        {
            ActivateSubtractors();
            ActivateAdders();
        }
    }
}
