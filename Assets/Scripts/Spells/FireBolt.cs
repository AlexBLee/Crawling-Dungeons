using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : Spell
{
    public override void ApplyDebuffs(CharacterEntity entity)
    {
        Debug.Log("FIRE!");
        entity.def -= 3;
        entity.gameObject.GetComponent<Renderer>().material.color = Color.red;
        GameObject.FindObjectOfType<BattleManager>().BeginWait();
    }
}
