﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[System.Serializable]
public class EnemyData
{
    public string name;
    public int maxHP;
    public int maxMP;
    public int str;
    public int intl;
    public int dex;
    public int luck;
    public int def;
    public int exp;
    public int gold;
}

public class GameDatabase : MonoBehaviour
{
    public static GameDatabase instance;

    [SerializeField]
    private TextAsset enemyJson = null;

    [SerializeField]
    private TextAsset levelJson = null;

    [SerializeField]
    private TextAsset armorJson = null;

    [SerializeField]
    private TextAsset weaponJson = null;

    [SerializeField]
    private TextAsset potionJson = null;

    [SerializeField]
    private TextAsset spellJson = null;

    [SerializeField]
    private TextAsset shopListJson = null;

    private Dictionary<string, EnemyData> enemyData = new Dictionary<string, EnemyData>();
    private Dictionary<string, Item> itemData = new Dictionary<string, Item>();
    private Dictionary<string, Spell> spellData = new Dictionary<string, Spell>();

    private List<string[]> levelData = new List<string[]>();
    private List<string> shopListData = new List<string>();

    [SerializeField]
    private SpriteAtlas spriteAtlas = null;

    protected void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        InitializeData();
    }

    private void InitializeData()
    {
        InitializeEnemyData();
        InitializeItemData();
        InitializeSpellData();
        levelData = JsonConvert.DeserializeObject<List<string[]>>(levelJson.text);
        shopListData = JsonConvert.DeserializeObject<List<string>>(shopListJson.text);
    }

    private void InitializeEnemyData()
    {
        EnemyData[] data = JsonConvert.DeserializeObject<EnemyData[]>(enemyJson.text);

        foreach (var enemy in data)
        {
            enemyData.Add(enemy.name, enemy);
        }
    }

    private void InitializeSpellData()
    {
        JArray data = JArray.Parse(spellJson.text);

        foreach (var spell in data)
        {
            var spellResult = SpellFactory.GetSpell(spell);

            spellData.Add(spellResult.name, spellResult);
        }
    }

    private void InitializeItemData()
    {
        JArray armorData = JArray.Parse(armorJson.text);
        JArray weaponData = JArray.Parse(weaponJson.text);
        JArray potionData = JArray.Parse(potionJson.text);

        armorData.Merge(weaponData);
        armorData.Merge(potionData);

        foreach (var item in armorData)
        {
            var equippableItem = ItemFactory.CreateItem(item);

            itemData.Add(equippableItem.itemName, equippableItem);
        }
    }

    public List<string> GetShopListData()
    {
        return shopListData;
    }

    public Item GetItemData(string itemName)
    {
        if (itemData.ContainsKey(itemName))
        {
            return itemData[itemName];
        }

        return null;
    }

    public EnemyData GetEnemyData(string name)
    {
        if (enemyData.ContainsKey(name))
        {
            return enemyData[name];
        }

        return null;
    }

    public Dictionary<string, Spell> GetSpellData()
    {
        return spellData;
    }

    public string[] GetLevelData(int levelNumber)
    {
        return levelData[levelNumber - 1];
    }

    public Sprite GetSprite(string itemName)
    {
        return spriteAtlas.GetSprite(itemName);
    }
}
