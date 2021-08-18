using UnityEngine;

public class Explosion : Spell
{
    public override void ApplyDebuffs(CharacterEntity entity)
    {
        GameObject.FindObjectOfType<BattleManager>().BeginWait();
    }
}
