﻿#pragma warning disable CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatDisplayer : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI str, dex, intl, will, statPoints;

    // Start is called before the first frame update
    void Start()
    {
        UpdateStats();
    }

    public void UpdateStats()
    {
        statPoints.text = player.statPoints.ToString();
        UpdateStr();
        UpdateDex();
        UpdateInt();
        UpdateWill();
    }

    public void UpdateStr()
    {
        str.text = player.str.amount.ToString();
    }

    public void UpdateDex()
    {
        dex.text = player.dex.amount.ToString();
    }

    public void UpdateInt()
    {
        intl.text = player.intl.amount.ToString();
    }

    public void UpdateWill()
    {
        will.text = player.luck.amount.ToString();
    }



}
