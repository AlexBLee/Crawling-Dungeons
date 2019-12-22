using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;// Required when using Event data.


public class AddRemoveStat : MonoBehaviour, IPointerClickHandler
{
    // true = add point
    // false = subtract
    [SerializeField]
    private bool option = true;

    [SerializeField]
    private Player player;

    [SerializeField]
    private int number;

    [SerializeField, HideInInspector]
    private StatDisplayer statDisplay;

    [SerializeField, HideInInspector]
    private UIManager uiManager;

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
