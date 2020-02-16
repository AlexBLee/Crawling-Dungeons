using System.Collections;
using UnityEngine;

public class Player : CharacterEntity
{
    [HideInInspector] public Inventory inventory;
    [HideInInspector] public Spells spells;
    public enum StatType { Str, Dex, Intl, Luck };

    private void Awake() 
    {
        inventory = GetComponent<Inventory>();
        spells = GetComponent<Spells>();

        ApplyStatsFrom(GameManager.instance.playerStats);
    }

    private void Start()
    {
        if(uiManager != null)
        {
            uiManager.UpdateUIHealth();
            uiManager.UpdateUIMana();
        }

        UpdateDamageStats();
        spells.UnlockSpells();
        inventory.UpdateItemStats();
    }

    private void Update()
    {
        if (inBattle && battleManager.playerTurn)
        {
            if (target != null)
            {     
                MoveAndAttack(target.transform.position);
            }
            StopAttacking();  
        }
    }

    // -----------------------------------------------------------------------------

    public void Attack()
    {
        uiManager.DisableButtons();
        initialPos = transform.position;
        targetReached = false;
        attacking = true;
    }

    public void MagicPressed(Spell spell)
    {
        if((mp - spell.cost) < 0)
        {
            Debug.Log("not enough mana!");
            infoText.color = new Color(0,205,255);
            infoText.text = "Not enough mana!";
            Instantiate(infoText, transform.position, Quaternion.identity);
        }
        else
        {
            AudioManager.Instance.Play(spell.effect.name);
            uiManager.HideMagicList();

            Debug.Log("Casted: " + spell.name);
            additionalDamage = spell.additionalDamage;
            mp -= spell.cost;
            uiManager.UpdateUIMana();

            attacking = true;
            RangedAttack();

            // Asks if the spell is supposed to be spawned firstly near the player or right on top of the enemy.
            if(spell.atPosition)
            {
                Instantiate(spell.effect, spells.spellPosition, Quaternion.identity);            
            }
            else
            {
                Instantiate(spell.effect, target.transform.position, Quaternion.identity);
            }
        }
    }

    #region Stats

    public bool AllocatePoints(StatType type)
    {
        switch (type)
        {
            case StatType.Str:
                str = AddStat(str);
                break;
            case StatType.Dex:
                dex = AddStat(dex);
                break;
            case StatType.Intl:
                intl = AddStat(intl);
                break;
            case StatType.Luck:
                luck = AddStat(luck);
                break;
            default:
                break;
        }

        statPoints--;
        UpdateDamageStats();
        return true;
    }

    public bool DeallocatePoints(StatType type)
    {
        switch (type)
        {
            case StatType.Str:
                str = RemoveStat(str);
                if(str.pointsAllocated == 0)
                {
                    statPoints++;
                    return str.modified = false;
                }
                break;
            case StatType.Dex:
                dex = RemoveStat(dex);
                if(dex.pointsAllocated == 0)
                {
                    statPoints++;
                    return dex.modified = false;
                }
                break;
            case StatType.Intl:
                intl = RemoveStat(intl);
                if(intl.pointsAllocated == 0)
                {
                    statPoints++;
                    return intl.modified = false;
                }
                break;
            case StatType.Luck:
                luck = RemoveStat(luck);
                if(luck.pointsAllocated == 0)
                {
                    statPoints++;
                    return luck.modified = false;
                }
                break;
            default:
                return false;
        }

        statPoints++;
        UpdateDamageStats();
        return true;

    }

    private Stat AddStat(Stat stat)
    {
        stat.amount++;
        stat.pointsAllocated++;
        stat.modified = true;
        return stat;
    }

    private Stat RemoveStat(Stat stat)
    {
        stat.amount--;
        stat.pointsAllocated--;
        return stat;
    }

    #endregion


    public void RecieveXPAndGold(int expRecieved, int goldRecieved)
    {
        // Randomized gold - to vary playstyle
        int randomGold = Random.Range(goldRecieved - 10, goldRecieved + 15);
        inventory.gold += randomGold;
        exp += expRecieved;
        infoText.color = new Color(255,255,255);
        infoText.text = "+" + expRecieved.ToString() + " XP";
        Instantiate(infoText, transform.position, Quaternion.identity);
        StartCoroutine(CheckForLevelUp());
        StartCoroutine(NextBattle());
    }

    private IEnumerator CheckForLevelUp()
    {
        while (exp >= expThreshold)
        {
            float extraXP = exp - expThreshold;
            LevelUp();
            spells.UnlockSpells();
            inventory.UpdateItemStats();

            yield return new WaitForSeconds(0.15f);
            uiManager.healthBar.SetAmount(hp,maxHP);
            uiManager.manaBar.SetAmount(mp,maxMP);
            infoText.text = "Level up!";
            Instantiate(infoText, transform.position, Quaternion.identity);

            exp += extraXP;
        }
    }

    private IEnumerator NextBattle()
    {
        yield return new WaitForSeconds(3);
        anim.SetBool("Run", true);
        battleManager.battleDone = true;
        battleManager.ToggleNextBattle();
    }

    public override void FinishDeath()
    {
        uiManager.ActivateGameOver();
        base.FinishDeath();
    }


    // -----------------------------------------------------------------------------

}
