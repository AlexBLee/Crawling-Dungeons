using UnityEngine;

[System.Serializable]
public class SpellInfo
{
    public string name;
    public int damage;
    public int cost;
    public int levelRequired;
    public int turnsLeft;

    public void InstantiateSpell(CharacterEntity entity)
    {
        GameObject spellPrefab = GameDatabase.instance.GetSpellPrefab(name);
        GameObject.Instantiate(spellPrefab, entity.transform.position, Quaternion.identity);
    }

    public virtual void UseSpellEffect(CharacterEntity entity)
    {
    }

    public virtual void UndoEffect(CharacterEntity entity)
    {
    }
}
