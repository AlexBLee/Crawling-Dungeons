using UnityEngine;
using DG.Tweening;

public class Enemy : CharacterEntity
{
    public string enemyName;
    
    public int ExperiencePoints;
    public int Gold;

    [SerializeField] private Vector3 fightPosition;
    [SerializeField] private bool canHeal;
    private int healCounter = 2;

    private void Start()
    {
        var enemyData = GameDatabase.instance.GetEnemyData(enemyName);
        
        _characterBattleStats.DeserializeEnemyData(enemyData);
        _characterBattleStats.UpdateDamageStats();
        
        ExperiencePoints = enemyData.exp;
        Gold = enemyData.gold;

        MoveToStartPosition(fightPosition);
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

        if (_characterBattleStats.Hp < _characterBattleStats.MaxHp * SelfLowHealthPercentage && canHeal)
        {
            canHeal = false;
            healCounter = HealCounterDefault;
            _characterBattleStats.Heal((int)(_characterBattleStats.MaxHp * HealFactor), false);
        }
        else if (target.CharacterBattleStats.Hp < target.CharacterBattleStats.MaxHp * EnemyLowHealthPercentage)
        {
            //TODO: Maybe dont need this.
            // additionalDamage = (int)(maxDamage * BonusDamage);
            _damageDealer.RangedAttack();
        }
        else
        {
            MoveToAttackPosition(target.transform.position);
        }
    }

    public override void FinishDeath()
    {
        target.GetComponent<Player>().RecieveXPAndGold(ExperiencePoints, Gold);
        base.FinishDeath();
    }



}
