[System.Serializable]
public class Spell
{
    public string name;
    public int damage;
    public int cost;
    public int levelRequired;
    public bool unlocked = false;

    public virtual void ApplyDebuffs(CharacterEntity entity)
    {
        
    }
}
