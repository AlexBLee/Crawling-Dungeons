[System.Serializable]
public abstract class Spell
{
    public string name;
    public int damage;
    public int cost;
    public int levelRequired;
    public bool unlocked = false;

    public abstract void ApplyDebuffs(CharacterEntity entity);
}
