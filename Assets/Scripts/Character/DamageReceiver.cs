using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] private AnimationPlayer _animationPlayer;
    [SerializeField] private CharacterBattleStats _characterBattleStats;
    
    public void ReceiveDamage(int damage, bool isCritting)
    {
        if (TargetDodged())
        {
            return;
        }
        
        _characterBattleStats.RemoveHealth(damage);
        
        _animationPlayer.PlayAnimationTrigger(CharacterClipAnims.HitAnimName);
        _animationPlayer.PlayBoolAnimation(CharacterClipAnims.GuardAnimName, false);
        
        AudioManager.Instance.PlaySound(AudioStrings.Hit);
        
        Managers.Instance.UI.SpawnInfoText(damage.ToString(), 
            isCritting 
                ? Color.yellow 
                : Color.white, transform.position);

        _characterBattleStats.CheckDeath();
    }
    
    private bool TargetDodged()
    {
        const int RandomMin = 0;
        const int RandomMax = 100;
        float missChance = Random.Range(RandomMin, RandomMax);

        if (missChance > _characterBattleStats.DodgeChance)
        {
            return false;
        }
        
        Managers.Instance.UI.SpawnInfoText(DisplayStrings.MissText, Color.white, transform.position);
        AudioManager.Instance.PlaySound(AudioStrings.Miss);

        return true;

    }
}
