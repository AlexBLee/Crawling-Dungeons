using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatDisplayer : MonoBehaviour
{
    private Player player;
    public TextMeshProUGUI str;
    public TextMeshProUGUI dex;
    public TextMeshProUGUI intl;
    public TextMeshProUGUI will;
    public TextMeshProUGUI statPoints;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.playerObject;
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
        str.text = player.str.ToString();
    }

    public void UpdateDex()
    {
        dex.text = player.dex.ToString();
    }

    public void UpdateInt()
    {
        intl.text = player.intl.ToString();
    }

    public void UpdateWill()
    {
        will.text = player.will.ToString();
    }



}
