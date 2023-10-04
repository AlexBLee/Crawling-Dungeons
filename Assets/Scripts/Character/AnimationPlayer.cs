using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void PlayAnimationTrigger(string animName)
    {
        _animator.SetTrigger(animName);
    }
    
    public void PlayBoolAnimation(string animName, bool val)
    {
        _animator.SetBool(animName, val);
    }
}
