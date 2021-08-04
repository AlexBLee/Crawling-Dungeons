using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellFactory
{
    public SpellNew GetSpell(string type)
    {
        switch (type)
        {
            case GameConstants.IceSpellName:
                return new IceBolt();

            case GameConstants.FireSpellName:
                return new FireBolt();

            default:
                return null;
        }
    }
}
