using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : Spell
{
    public override void ApplyDebuffs(CharacterEntity entity)
    {
        Debug.Log("FIRE!");
        entity.def -= 3;
        GameObject.Instantiate(prefab, entity.transform.position, Quaternion.identity);
    }
}
