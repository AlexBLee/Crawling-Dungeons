using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBolt : SpellNew
{
    public override string Name {get { return "Ice"; }}
    public override int Damage {get { return 15; }}
    public override int Cost { get { return 5; }}
    public override int LevelRequired { get { return 1; }}

    public override void ApplyDebuffs(CharacterEntity entity)
    {
        Debug.Log("Ice!");
        entity.def -= 5;
        entity.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        GameObject.FindObjectOfType<BattleManager>().BeginWait();


    }
}
