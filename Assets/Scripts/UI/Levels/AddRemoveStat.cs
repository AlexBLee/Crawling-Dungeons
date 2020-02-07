#pragma warning disable CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;// Required when using Event data.
using UnityEngine.UI;


public class AddRemoveStat : MonoBehaviour
{
    // true = add point
    // false = subtract
    [SerializeField] private Player player;
    [SerializeField] private int number;
    [SerializeField] public static int numberOfStatPoints;
    public Button plus;
    public Button minus;
    public bool modified;

    [SerializeField] private StatDisplayer statDisplay;
    [SerializeField] private UIManager uiManager;

    private void Start() 
    {
        plus.onClick.AddListener(AddStat);
        minus.onClick.AddListener(RemoveStat);
    }

    private void AddStat()
    {
        AudioManager.Instance.Play("AddClick");
        modified = player.AddToStat(number);
        statDisplay.UpdateStats();
        CheckStatAmount();
        Debug.Log(modified);
    }

    private void RemoveStat()
    {
        AudioManager.Instance.Play("RemoveClick");
        modified = player.RemoveFromStat(number);
        statDisplay.UpdateStats();
        CheckStatAmount();
        Debug.Log(modified);

    }


    private void CheckStatAmount()
    {
        if(player.statPoints == 0)
        {
            uiManager.DeactivateAdders();
            uiManager.ActivateSubtractors();
        }
        else if(player.statPoints == numberOfStatPoints)
        {
            uiManager.DeactivateSubtractors();
            uiManager.ActivateAdders();
        }
        else if(player.statPoints > 0)
        {
            uiManager.ActivateSubtractors();
            uiManager.ActivateAdders();
        }
    }
}
