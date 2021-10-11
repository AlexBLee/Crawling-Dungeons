﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using JetBrains.Annotations;

// the class for all Characters in the game.
public class CharacterEntity : MonoBehaviour
{
    // Managers ------------------------------
    public BattleManager battleManager;
    public UIManager uiManager;

    // Stats ----------------------------------
    public int level;
    public float exp;
    public float expThreshold;
    public int statPoints;

    public int hp, mp;
    public int maxHP, maxMP;
    public Stat str, intl, dex, luck;

    [SerializeField] protected int additionalDamage;
    [SerializeField] protected float critChance;
    [SerializeField] protected float dodgeChance;

    public float minDamage, maxDamage;
    public float magicMinDamage, magicMaxDamage;

    public int def;

    // Conditions --------------------------
    public bool inBattle;
    protected bool dead = false;

    // Others ----------------------------
    
    [HideInInspector] public Animator anim;
    protected Vector3 initialPos;
    public CharacterEntity target;
    public Spell spellUsed;
    private bool isCritting = false;


    // ------------------------------------

    #region Stats  

    protected void UpdateDamageStats()
    {
        minDamage = (str.amount / PlayerDefaultConstants.MinDamageStrCalc);
        maxDamage = (str.amount / PlayerDefaultConstants.MaxDamageStrCalc);

        minDamage += (dex.amount / PlayerDefaultConstants.MinDamageDexCalc);
        maxDamage += (dex.amount / PlayerDefaultConstants.MaxDamageDexCalc);

        magicMinDamage = (intl.amount / PlayerDefaultConstants.MagicMinDamageCalc);
        magicMaxDamage = (intl.amount / PlayerDefaultConstants.MagicMaxDamageCalc);

        critChance = (luck.amount / PlayerDefaultConstants.CritChanceCalc);
        dodgeChance = (dex.amount / PlayerDefaultConstants.DodgeChanceCalc);

        def = (str.amount / PlayerDefaultConstants.DefCalc);
    }

    private void CheckDeath()
    {
        if (hp <= 0 && inBattle)
        {
            dead = true;
            anim.SetTrigger("Death");
        }
    }

    public virtual void FinishDeath()
    {
        Destroy(gameObject);
    }
    
    #endregion

    #region Moves
    
    [UsedImplicitly]
    public void DoDamage()
    {
        if (TargetDodged())
        {
            return;
        }

        target.RecieveDamage(CalculateInitialDamage(minDamage, maxDamage, additionalDamage));

        additionalDamage = 0;
    }

    [UsedImplicitly]
    protected void DoMagicDamage()
    {
        if (TargetDodged())
        {
            battleManager.BeginWait();
            return;
        }

        target.RecieveDamage(CalculateInitialDamage(magicMinDamage, magicMaxDamage, spellUsed.damage));

        spellUsed.UseSpellEffect(target);

        spellUsed = null;
    }

    private bool TargetDodged()
    {
        const int RandomMin = 0;
        const int RandomMax = 100;
        float missChance = Random.Range(RandomMin, RandomMax);
        
        if (missChance < target.dodgeChance)
        {
            uiManager.SpawnInfoText("Miss!", Color.white, target.transform.position);
            AudioManager.Instance.PlaySound(AudioStrings.Miss);

            return true;
        }

        return false;
    }

    private int CalculateInitialDamage(float minimumDamage, float maximumDamage, float extraDamage)
    {
        const float DefStartPointCalc = 1f;
        const float ConvertToPercentageCalc = 100;

        float damage = Random.Range(minimumDamage, maximumDamage);

        damage *= (DefStartPointCalc - (target.def / ConvertToPercentageCalc));

        damage += extraDamage;

        damage = CalculateCritDamage(damage);

        return Mathf.RoundToInt(damage);
    }

    private float CalculateCritDamage(float damage)
    {
        const int CritBonusDamage = 2;
        const int RandomMin = 0;
        const int RandomMax = 100;

        float chance = Random.Range(RandomMin, RandomMax);

        if (chance < critChance)
        {
            damage *= CritBonusDamage;
            
            if (target is Player player)
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

            isCritting = true;
        }
        else
        {
            isCritting = false;
        }

        return damage;
    }

    protected virtual void RecieveDamage(int damage)
    {
        hp -= damage;
        anim.SetTrigger("Hit");
        AudioManager.Instance.PlaySound(AudioStrings.Hit);

        uiManager.SpawnInfoText(damage.ToString(), 
            target.isCritting 
                ? Color.yellow 
                : Color.white, transform.position);
        
        CheckDeath();
    }

    // Next turn setting + damage is done through animation
    protected void RangedAttack()
    {
        uiManager.DisableButtons();
        anim.SetTrigger("Cast");
    }

    public virtual void Heal(int amount, bool battleFinish)
    {
        hp += amount;

        if (hp >= maxHP)
        {
            hp = maxHP;
        }

        if (!battleFinish)
        {
            uiManager.SpawnInfoText(amount.ToString(), Color.green, transform.position);
            AudioManager.Instance.PlaySound(AudioStrings.UsePotion);
            anim.SetTrigger("Heal");
        }
    }

    public virtual void RestoreMP(int amount, bool battleFinish)
    {
        mp += amount;

        if (mp >= maxMP)
        {
            mp = maxMP;
        }

        if (!battleFinish)
        {
            uiManager.SpawnInfoText(amount.ToString(), Color.cyan, transform.position);
            AudioManager.Instance.PlaySound(AudioStrings.UsePotion);
            anim.SetTrigger("Heal");
        }
    }

    #endregion

    #region Animations/Movement
   
    protected void MoveToAttackPosition(Vector2 targetPosition)
    {
        anim.SetBool("Run", true);

        float modifiedTargetPos = targetPosition.x - transform.right.x;
        transform.DOMoveX(modifiedTargetPos, 1).OnComplete(StartAttack);
    }

    private void StartAttack()
    {
        anim.SetBool("Run", false);
        anim.SetTrigger("Attack");
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
        StartCoroutine(battleManager.ToggleNextTurn());
    }
}
