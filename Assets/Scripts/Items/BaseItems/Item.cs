using UnityEngine;

public class Item
{
    public string itemName;
    public string description;
    public Sprite image;
    public int cost;
    [HideInInspector] public int amount;

    public void SetImage()
    {
        image = GameDatabase.instance.GetSprite(itemName);
    }
}
