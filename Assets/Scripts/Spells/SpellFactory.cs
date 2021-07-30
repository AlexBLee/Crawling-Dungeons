using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellFactory
{
    public SpellNew GetSpell(string type)
    {
        switch (type)
        {
            case "fire":
                return new FireBolt();

            case "ice":
                return new IceBolt();

            default:
                return null;
        }
    }
}
