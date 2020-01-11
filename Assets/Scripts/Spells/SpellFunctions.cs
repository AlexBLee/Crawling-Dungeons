using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// These functions are only called by the animation of each spell.
public class SpellFunctions : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void PlaySound(string n)
    {
        AudioManager.Instance.Play(n);
    }

    public void MagicAnimationFinish()
    {
        FindObjectOfType<BattleManager>().BeginWait();
    }
}
