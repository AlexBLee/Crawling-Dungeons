using UnityEngine;

public class Xplosion : Spell
{
    public override void ApplyDebuffs(CharacterEntity entity)
    {
        GameObject.Instantiate(prefab, entity.transform.position, Quaternion.identity);
    }
}
