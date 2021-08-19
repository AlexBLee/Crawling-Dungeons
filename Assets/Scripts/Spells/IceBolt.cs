using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBolt : Spell
{
    public override void ApplyDebuffs(CharacterEntity entity)
    {
        Debug.Log("Ice!");
        entity.def -= 5;
        entity.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        GameObject.Instantiate(prefab, entity.transform.position, Quaternion.identity);
    }
}
