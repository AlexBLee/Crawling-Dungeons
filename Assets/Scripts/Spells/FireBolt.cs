using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : SpellNew
{
    public override string Name { get { return "FireBolt"; }}
    public override int Damage { get { return 25; }}
    public override int Cost { get { return 10; }}



    public override void ApplyDebuffs(CharacterEntity entity)
    {
        Debug.Log("FIRE!");
        entity.def -= 3;
        entity.gameObject.GetComponent<Renderer>().material.color = Color.red;
        GameObject.FindObjectOfType<BattleManager>().BeginWait();


    }
}
