using UnityEngine;

public class EnergyBall : Spell
{
    public override void ApplyDebuffs(CharacterEntity entity)
    {
        GameObject.FindObjectOfType<BattleManager>().BeginWait();
    }
}
