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

    private Dictionary<string, EnemyData> enemyData = new Dictionary<string, EnemyData>();
    private Dictionary<string, LevelData> levelData = new Dictionary<string, LevelData>();

    private List<string[]> levelDatas = new List<string[]>();

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

    public EnemyData GetEnemyData(string name)
    {
        if (enemyData.ContainsKey(name))
        {
            return enemyData[name];
        }

        return null;
    }

    public string[] GetLevelData(int index)
    {
        return levelDatas[index];
    }
}
