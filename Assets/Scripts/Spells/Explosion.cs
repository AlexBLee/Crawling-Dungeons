using UnityEngine;

public class Explosion : Spell
{
    public override void ApplyDebuffs(CharacterEntity entity)
    {
        GameObject.Instantiate(prefab, entity.transform.position, Quaternion.identity);
        GameObject.FindObjectOfType<BattleManager>().BeginWait();
    }
}
