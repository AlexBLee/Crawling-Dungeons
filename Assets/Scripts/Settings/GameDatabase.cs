using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public class EnemyObj
{
    public EnemyData[] Enemies;
}

public class GameDatabase : MonoBehaviour
{
    public static GameDatabase instance;

    [SerializeField]
    private TextAsset enemyJson;
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
    }

    public void LoadEnemyData(string json)
    {
        enemyData.Clear();
        EnemyObj data = JsonUtility.FromJson<EnemyObj>(json);

        foreach (EnemyData enemy in data.Enemies)
        {
            enemyData.Add(enemy.name, enemy);
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
}
