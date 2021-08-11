using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

[System.Serializable]
public class LevelData
{
    public string name;
    public string[] enemies;
}

public class GameDatabase : MonoBehaviour
{
    public static GameDatabase instance;

    [SerializeField]
    private TextAsset enemyJson;

    [SerializeField]
    private TextAsset levelJson;

    [SerializeField]
    private TextAsset armorJson;

    [SerializeField]
    private TextAsset weaponJson;

    private Dictionary<string, EnemyData> enemyData = new Dictionary<string, EnemyData>();
    private Dictionary<string, LevelData> levelData = new Dictionary<string, LevelData>();
    private Dictionary<string, ArmorItem> armorData = new Dictionary<string, ArmorItem>();
    private Dictionary<string, WeaponItem> weaponData = new Dictionary<string, WeaponItem>();

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
        InitializeArmorData();
        InitializeWeaponData();
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
        LevelData[] data = LoadJsonData<LevelData>(levelJson.text);

        foreach (var level in data)
        {
            levelData.Add(level.name, level);
        }
    }

    private void InitializeArmorData()
    {
        ArmorItem[] data = LoadJsonData<ArmorItem>(armorJson.text);

        foreach (var item in data)
        {
            armorData.Add(item.itemName, item);
        }
    }

    private void InitializeWeaponData()
    {
        WeaponItem[] data = LoadJsonData<WeaponItem>(weaponJson.text);

        foreach (var item in data)
        {
            weaponData.Add(item.itemName, item);
        }
    }

    public EnemyData GetEnemyData(string name)
    {
        if (enemyData.ContainsKey(name))
        {
            return enemyData[name];
        }

        return null;
    }

    public LevelData GetLevelData(int levelNumber)
    {
        string levelName = "Level " + levelNumber.ToString();

        return levelData[levelName];
    }
}
