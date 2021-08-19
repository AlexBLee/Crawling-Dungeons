using UnityEngine;

[System.Serializable]
public class Spell
{
    public string name;
    public int damage;
    public int cost;
    public int levelRequired;

    public GameObject prefab;
    public bool unlocked = false;

    public void SetPrefab(GameObject gameObject)
    {
        prefab = gameObject;
    }

    public virtual void ApplyDebuffs(CharacterEntity entity)
    {
        
    }
}
