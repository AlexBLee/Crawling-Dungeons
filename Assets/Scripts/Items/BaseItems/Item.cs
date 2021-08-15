using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item
{
    public string itemName;
    public string description;
    public string imageName;
    public Sprite image;
    public int cost;
    [HideInInspector] public int amount;

    public void SetImage()
    {
        image = GameDatabase.instance.GetSprite(itemName);
    }
}
