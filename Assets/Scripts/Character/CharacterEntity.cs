using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// the class for all Characters in the game.
public class CharacterEntity : MonoBehaviour
{
    [SerializeField] protected DamageDealer _damageDealer;
    [SerializeField] protected DamageReceiver _damageReceiver;
    [SerializeField] protected CharacterBattleStats _characterBattleStats;

    // Conditions --------------------------
    protected bool dead = false;
    private bool _guarding;

    // Others ----------------------------
    
    [HideInInspector] public Animator anim;
    protected Vector3 initialPos;
    public CharacterEntity target;
    public Spell spellUsed;
    private bool isCritting = false;
    protected int _statPoints;
    protected int _originalNumberOfStatPoints = 0;


    public List<Spell> spellModifiers = new List<Spell>();

    public DamageDealer DamageDealer => _damageDealer;
    public DamageReceiver DamageReceiver => _damageReceiver;
    public CharacterBattleStats CharacterBattleStats => _characterBattleStats;
    public int StatPoints => _statPoints;

    public bool Guarding => _guarding;

    // ------------------------------------
    
    public virtual void FinishDeath()
    {
        Destroy(gameObject);
    }

    public virtual void Guard()
    {
        _guarding = true;

        Managers.Instance.UI.SpawnInfoText(DisplayStrings.GuardingText, Color.white, transform.position);
        Managers.Instance.UI.DisableButtons();
        anim.SetBool(CharacterClipAnims.GuardAnimName, true);
        ToggleNextTurn();
    }
    
    public void CheckStatAmount()
    {
        if (_statPoints == 0)
        {
            Managers.Instance.UI.VictoryPanelHUD.ActivateModifiedSubtractors();
        }
        else if (_statPoints == _originalNumberOfStatPoints)
        {
            Managers.Instance.UI.VictoryPanelHUD.ActivateAddersOnly();
        }
        else if (_statPoints > 0)
        {
            Managers.Instance.UI.VictoryPanelHUD.ActivateAllStatModifiers();
        }
    }

    #region Animations/Movement
   
    protected void MoveToAttackPosition(Vector2 targetPosition)
    {
        anim.SetBool(CharacterClipAnims.RunAnimName, true);

        float modifiedTargetPos = targetPosition.x - transform.right.x;
        transform.DOMoveX(modifiedTargetPos, 1).OnComplete(StartAttack);
    }

    private void StartAttack()
    {
        anim.SetBool(CharacterClipAnims.RunAnimName, false);
        anim.SetTrigger(CharacterClipAnims.AttackAnimName);
    }

    // Animation Finishes - triggered in animations.
    public void AnimationFinish()
    {
        transform.DOMove(initialPos, 1);
        ToggleNextTurn();
    }

    public void MagicAnimationFinish()
    {
        ToggleNextTurn();
    }

    #endregion

    public void PlaySound(string n)
    {
        AudioManager.Instance.PlaySound(n);
    }
    
    private void ToggleNextTurn()
    {
        _guarding = false;
        StartCoroutine(Managers.Instance.Battle.ToggleNextTurn());
    }

    public bool CheckIfSpellExists(string name)
    {
        return spellModifiers.Exists(spell => spell.Name == name);
    }

    public void ApplySpells()
    {
        foreach (Spell spell in spellModifiers)
        {
            spell.TurnsLeft--;
            Debug.Log($"{spell.Name}: {spell.TurnsLeft}");

            if (spell.TurnsLeft == 0)
            {
                spell.UndoEffect(this);
            }
        }

        spellModifiers.RemoveAll(spell => spell.TurnsLeft <= 0);
    }
}
