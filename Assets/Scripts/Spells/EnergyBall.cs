using UnityEngine;

public class EnergyBall : Spell
{
    public override void ApplyDebuffs(CharacterEntity entity)
    {
        GameObject.Instantiate(prefab, entity.target.transform.position, Quaternion.identity);
    }
}
