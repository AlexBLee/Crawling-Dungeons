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

    void Awake()
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
        LoadData(levelJson.text);
    }

    public void LoadEnemyData(string json)
    {
        enemyData.Clear();
        EnemyData[] data = JsonConvert.DeserializeObject<EnemyData[]>(json);

        foreach (EnemyData enemy in data)
        {
            enemyData.Add(enemy.name, enemy);
        }
    }

    public void LoadData(string json)
    {
        string[][] vv = JsonConvert.DeserializeObject<string[][]>(json);

    }

    public EnemyData GetEnemyData(string name)
    {
        if (enemyData.ContainsKey(name))
        {
            return enemyData[name];
        }

        return null;
    }
}
