using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellNew
{
    public abstract string Name { get; }
    public abstract int Damage { get; }
    public abstract int Cost { get; }
    public abstract int LevelRequired { get; }
    public bool unlocked = false;
    public abstract void ApplyDebuffs(CharacterEntity entity);
}
