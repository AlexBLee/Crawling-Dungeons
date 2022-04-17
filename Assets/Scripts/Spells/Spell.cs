using UnityEngine;

[System.Serializable]
public class Spell
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
        turnsLeft -= 1;
    }

    public virtual void UndoEffect(CharacterEntity entity)
    {
    }
}
