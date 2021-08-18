using UnityEngine;

public class GodsMight : Spell
{
    public override void ApplyDebuffs(CharacterEntity entity)
    {
        GameObject.FindObjectOfType<BattleManager>().BeginWait();
    }
}
