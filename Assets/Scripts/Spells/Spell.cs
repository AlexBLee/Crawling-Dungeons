using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell/Spell")]
public class Spell : ScriptableObject
{
    public string spellName;
    public int cost;
    public int additionalDamage;
    public GameObject effect;





}
