﻿using UnityEngine;

public class IceBolt : Spell
{
    public override void UseSpellEffect(CharacterEntity entity)
    {
        base.UseSpellEffect(entity);

        entity.def -= 5;
        entity.gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }
}
