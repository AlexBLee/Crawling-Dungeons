public class SpellStateInfo
{
    private string id = "";
    private bool unlocked = false;

    public SpellStateInfo(string spellId)
    {
        id = spellId;
    }

    public string GetID()
    {
        return id;
    }

    public bool GetUnlockedState()
    {
        return unlocked;
    }

    public void SetUnlocked()
    {
        if (!unlocked)
        {
            unlocked = true;
        }
    }
}
