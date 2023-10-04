using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private CharacterBattleStats _characterBattleStats;
    [SerializeField] private AnimationPlayer _animationPlayer;

    private Spell _spellUsed;
    private CharacterEntity _target;
    private bool _isCritting = false;

    public bool IsCritting => _isCritting;
    
    [UsedImplicitly]
    public void DoDamage()
    {
        _target.DamageReceiver.ReceiveDamage(
            CalculateInitialDamage(
                _characterBattleStats.MinDamage, 
                _characterBattleStats.MaxDamage, 
                _characterBattleStats.AdditionalDamage), _isCritting);
    }

    [UsedImplicitly]
    protected void DoMagicDamage()
    {
        var initialDamage = CalculateInitialDamage(
            _characterBattleStats.MagicMinDamage,
            _characterBattleStats.MagicMaxDamage, 
            _spellUsed.Damage);
        
        _target.DamageReceiver.ReceiveDamage(initialDamage, _isCritting);

        _spellUsed.InstantiateSpell(_target);

        if (!_target.CheckIfSpellExists(_spellUsed.Name))
        {
            _target.spellModifiers.Add(_spellUsed);

            _spellUsed.UseSpellEffect(_target);
        }
        else
        {
            Spell spell = SpellFactory.GetSpell(_spellUsed.Name);
            Spell existingSpell = _target.spellModifiers.Find(spell => _spellUsed.Name == spell.Name);

            existingSpell.TurnsLeft = spell.TurnsLeft;
        }

        _spellUsed = null;
    }

    private int CalculateInitialDamage(float minimumDamage, float maximumDamage, float extraDamage)
    {
        const float DefStartPointCalc = 1f;
        const float ConvertToPercentageCalc = 100;
        const float GuardingFactor = 0.66f;

        float damage = Random.Range(minimumDamage, maximumDamage);

        damage *= (DefStartPointCalc - (_target.def / ConvertToPercentageCalc));

        damage += extraDamage;

        damage = CalculateCritDamage(damage);

        if (_target.Guarding)
        {
            damage *= GuardingFactor;
        }

        return Mathf.RoundToInt(damage);
    }

    private float CalculateCritDamage(float damage)
    {
        const int CritBonusDamage = 2;
        const int RandomMin = 0;
        const int RandomMax = 100;

        float chance = Random.Range(RandomMin, RandomMax);

        if (chance < _characterBattleStats.CritChance)
        {
            damage *= CritBonusDamage;
            
            if (_target is Player player)
            {
                LeftHand shield = (LeftHand)player
                    .GetInventory()
                    .GetEquipList()
                    [(int)Inventory.EquipSlot.LeftHand];

                if (shield != null)
                {
                    damage = shield.ReduceCrit(damage);
                }
            }

            _isCritting = true;
        }
        else
        {
            _isCritting = false;
        }

        return damage;
    }

    // Next turn setting + damage is done through animation
    protected void RangedAttack()
    {
        Managers.Instance.UI.DisableButtons();
        _animationPlayer.PlayAnimationTrigger(CharacterClipAnims.CastAnimName);
    }
}
