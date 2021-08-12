using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Newtonsoft.Json;

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
    private TextAsset shopListJson = null;

    private Dictionary<string, EnemyData> enemyData = new Dictionary<string, EnemyData>();
    private Dictionary<string, Item> itemData = new Dictionary<string, Item>();

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
        InitializeLevelData();
        InitializeItemData();
        InitializeShopListData();
    }

    private T[] LoadJsonData<T>(string json)
    {
        return JsonConvert.DeserializeObject<T[]>(json);
    }

    private void InitializeEnemyData()
    {
        EnemyData[] data = LoadJsonData<EnemyData>(enemyJson.text);

        foreach (var enemy in data)
        {
            enemyData.Add(enemy.name, enemy);
        }
    }

    private void InitializeLevelData()
    {
        string[][] data = LoadJsonData<string[]>(levelJson.text);

        foreach (var level in data)
        {
            levelData.Add(level);
        }
    }

    private void InitializeItemData()
    {
        ArmorItem[] armorData = LoadJsonData<ArmorItem>(armorJson.text);
        WeaponItem[] weaponData = LoadJsonData<WeaponItem>(weaponJson.text);

        foreach (var item in armorData)
        {
            item.image = spriteAtlas.GetSprite(item.itemName);
            itemData.Add(item.itemName, item);
        }

        foreach (var item in weaponData)
        {
            item.image = spriteAtlas.GetSprite(item.itemName);
            itemData.Add(item.itemName, item);
        }
    }

    private void InitializeShopListData()
    {
        string[] data = LoadJsonData<string>(shopListJson.text);

        foreach (var item in data)
        {
            shopListData.Add(item);
        }
    }

    public List<string> GetShopListData()
    {
        return shopListData;
    }

    public Item GetItemData(string itemName)
    {
        return itemData[itemName];
    }

    public EnemyData GetEnemyData(string name)
    {
        if (enemyData.ContainsKey(name))
        {
            return enemyData[name];
        }

        return null;
    }

    public string[] GetLevelData(int levelNumber)
    {
        return levelData[levelNumber - 1];
    }
}
