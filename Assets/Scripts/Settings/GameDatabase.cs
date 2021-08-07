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

public class GameDatabase : MonoBehaviour
{
    public static GameDatabase instance;

    [SerializeField]
    private TextAsset enemyJson;

    [SerializeField]
    private TextAsset levelJson;

    private Dictionary<string, EnemyData> enemyData = new Dictionary<string, EnemyData>();
    private List<string[]> levelData = new List<string[]>();

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

        LoadEnemyData(enemyJson.text);
        LoadLevelData(levelJson.text);
    }

    private void LoadEnemyData(string json)
    {
        enemyData.Clear();
        EnemyData[] data = JsonConvert.DeserializeObject<EnemyData[]>(json);

        foreach (EnemyData enemy in data)
        {
            enemyData.Add(enemy.name, enemy);
        }
    }

    private void LoadLevelData(string json)
    {
        levelData.Clear();
        string[][] data = JsonConvert.DeserializeObject<string[][]>(json);

        foreach (string[] level in data)
        {
            levelData.Add(level);
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
        return levelData[index];
    }
}
