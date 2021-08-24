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
    [SerializeField] public static int numberOfStatPoints;
    [SerializeField] private Player.StatType statType;
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
        modified = player.AllocatePoints(statType);
        statDisplay.UpdateStats();
        player.CheckStatAmount();
    }

    private void RemoveStat()
    {
        AudioManager.Instance.Play("RemoveClick");
        modified = player.DeallocatePoints(statType);
        statDisplay.UpdateStats();
        player.CheckStatAmount();
    }

    public void ActivateAdder()
    {
        plus.gameObject.SetActive(true);
    }

    public void DeactivateAdder()
    {
        plus.gameObject.SetActive(false);
    }

    public void ActivateSubtractorIfModified()
    {
        minus.gameObject.SetActive(modified ? true : false);
    }

    public void DeactivateSubtractor()
    {
        minus.gameObject.SetActive(false);
    }
}
