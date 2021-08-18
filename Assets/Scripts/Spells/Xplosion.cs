using UnityEngine;

public class Xplosion : Spell
{
    public override void ApplyDebuffs(CharacterEntity entity)
    {
        GameObject.FindObjectOfType<BattleManager>().BeginWait();
    }
}
