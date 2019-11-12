using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;// Required when using Event data.


public class AddRemoveStat : MonoBehaviour, IPointerClickHandler
{
    // true = add point
    // false = subtract
    public bool option = true;
    public Player player;
    public int number;
    public StatDisplayer statDisplay;
    public UIManager uiManager;

    public void OnPointerClick(PointerEventData eventData)
    {

        if(option)
        {
            AudioManager.Instance.Play("AddClick");
            player.AddToStat(number);
            statDisplay.UpdateStats();
        }
        else
        {
            AudioManager.Instance.Play("RemoveClick");
            player.RemoveFromStat(number);
            statDisplay.UpdateStats();

        }
        
        if(player.statPoints == 0)
        {
            uiManager.DeactivateAdders();
        }
    }
}
