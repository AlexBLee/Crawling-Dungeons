using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Enemy : CharacterEntity
{
    public string enemyName;
    public int experiencePoints;
    public int gold;

    public bool newBattle;
    [SerializeField] private Vector3 fightPosition;
    [SerializeField] private bool canHeal;
    private int healCounter = 1;

    private const int HealCounterDefault = 2;
    private const float BonusDamage = 1.07f;
    private const float EnemyLowHealthPercentage = 0.40f;
    private const float SelfLowHealthPercentage = 0.30f;
    private const float HealFactor = 0.15f;

    private void Start() 
    {   
        uiManager = FindObjectOfType<UIManager>();
        
        DeserializeData();
        UpdateDamageStats();

        MoveToStartPosition(fightPosition);
    }

    private void DeserializeData()
    {
        EnemyData data = GameDatabase.instance.GetEnemyData(enemyName);
        
        maxHP = data.maxHP;
        hp = maxHP;

        maxMP = data.maxMP;
        mp = maxMP;

        str.amount = data.str;
        intl.amount = data.intl;
        dex.amount = data.dex;
        luck.amount = data.luck;

        def = data.def;
        experiencePoints = data.exp;
        gold = data.gold;
    }

    protected void MoveToStartPosition(Vector2 position)
    {
        anim.SetBool("Run", true);
        transform.DOMove(position, 1).OnComplete(StartBattle);
    }

    private void StartBattle()
    {
        initialPos = transform.position;

        anim.SetBool("Run", false);
        battleManager.StartNewBattle();
    }

    public void SetAttackConditions()
    {
        // The AI can only heal once every two turns.
        // The counter is there to keep count of how manys turns its been since it last healed.

        if(!dead)
        {
            if(!canHeal)
            {
                healCounter--;
            }
            if(healCounter == 0)
            {
                canHeal = true;
            }

            FindBestMove();
        }

        
    }

    public void FindBestMove()
    {
        // Heal if self is low on health and is able to heal
        if(hp < maxHP * SelfLowHealthPercentage && canHeal)
        {
            canHeal = false;
            healCounter = HealCounterDefault;
            Heal((int)(maxHP * HealFactor), false);
        }
        // Use spell if the player is low on health
        else if(target.hp < target.maxHP * EnemyLowHealthPercentage)
        {
            additionalDamage = (int)(maxDamage * BonusDamage);
            RangedAttack();
        }
        // Normal Attack
        else
        {
            MoveToAttackPosition(target.transform.position);
        }

    }

    public override void FinishDeath()
    {
        target.GetComponent<Player>().RecieveXPAndGold(experiencePoints, gold);
        base.FinishDeath();
    }



}
