using UnityEngine;
using DG.Tweening;

public class Enemy : CharacterEntity
{
    public string enemyName;
    public int experiencePoints;
    public int gold;

    [SerializeField] private Vector3 fightPosition;
    [SerializeField] private bool canHeal;
    private int healCounter = 2;

    private void Start() 
    {   
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
        anim.SetBool(CharacterClipAnims.RunAnimName, true);
        transform.DOMove(position, 1).OnComplete(StartBattle);
    }

    private void StartBattle()
    {
        initialPos = transform.position;

        anim.SetBool(CharacterClipAnims.RunAnimName, false);
        Managers.Instance.Battle.StartNewBattle();
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
        const int HealCounterDefault = 2;
        const float BonusDamage = 1.07f;
        const float EnemyLowHealthPercentage = 0.20f;
        const float SelfLowHealthPercentage = 0.30f;
        const float HealFactor = 0.10f;

        if (hp < maxHP * SelfLowHealthPercentage && canHeal)
        {
            canHeal = false;
            healCounter = HealCounterDefault;
            Heal((int)(maxHP * HealFactor), false);
        }
        else if (target.hp < target.maxHP * EnemyLowHealthPercentage)
        {
            additionalDamage = (int)(maxDamage * BonusDamage);
            RangedAttack();
        }
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
