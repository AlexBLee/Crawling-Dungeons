using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        FindObjectOfType<BattleManager>().ToggleNextTurn();
    }
}
