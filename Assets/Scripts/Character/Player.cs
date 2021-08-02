using System.Collections;
using UnityEngine;

public class Player : CharacterEntity
{
    [HideInInspector] public Inventory inventory;
    [HideInInspector] public Spells spells;
    public enum StatType { Str, Dex, Intl, Luck };

    private Color lightBlue = new Color(0,205,255);
    private const int goldMinValue = 10;
    private const int goldMaxValue = 15;
    private const float levelDelayTime = 0.15f;
    private const float battleDelayTime = 3;


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

    // -----------------------------------------------------------------------------

    public void Attack()
    {
        uiManager.DisableButtons();
        initialPos = transform.position;
        MoveToAttackPosition(target.transform.position);
    }

    public void MagicPressed(SpellNew spell)
    {
        if((mp - spell.Cost) < 0)
        {
            Debug.Log("not enough mana!");
            infoText.color = lightBlue;
            infoText.text = "Not enough mana!";
            Instantiate(infoText, transform.position, Quaternion.identity);
        }
        else
        {
            // AudioManager.Instance.Play(spell.effect.name);
            uiManager.HideMagicList();

            Debug.Log("Casted: " + spell.Name);
            spellUsed = spell;
            mp -= spellUsed.Cost;
            
            uiManager.UpdateUIMana();

            RangedAttack();

            // Asks if the spell is supposed to be spawned firstly near the player or right on top of the enemy.
            // if(spell.atPosition)
            // {
            //     Instantiate(spell.effect, spells.spellPosition, Quaternion.identity);            
            // }
            // else
            // {
            //     Instantiate(spell.effect, target.transform.position, Quaternion.identity);
            // }
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
        // exists to match the modified bool of the stat and return the correct value to the stat remover
        bool statZero = false;

        switch (type)
        {
            case StatType.Str:
                str = RemoveStat(str, statZero);
                statZero = str.modified;
                break;
            case StatType.Dex:
                dex = RemoveStat(dex, statZero);
                statZero = dex.modified;
                break;
            case StatType.Intl:
                intl = RemoveStat(intl, statZero);
                statZero = intl.modified;
                break;
            case StatType.Luck:
                luck = RemoveStat(luck, statZero);
                statZero = luck.modified;
                break;
            default:
                return false;
        }

        statPoints++;
        UpdateDamageStats();
        return statZero;

    }

    private Stat AddStat(Stat stat)
    {
        stat.amount++;
        stat.pointsAllocated++;
        stat.modified = true;
        return stat;
    }

    private Stat RemoveStat(Stat stat, bool result)
    {
        stat.amount--;
        stat.pointsAllocated--;

        if(stat.pointsAllocated == 0)
        {
            stat.modified = false;
        }
        return stat;
    }

    #endregion


    public void RecieveXPAndGold(int expRecieved, int goldRecieved)
    {
        // Randomized gold - to vary playstyle
        int randomGold = Random.Range(goldRecieved - goldMinValue, goldRecieved + goldMaxValue);
        inventory.gold += randomGold;
        exp += expRecieved;
        infoText.color = Color.white;
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

            yield return new WaitForSeconds(levelDelayTime);
            uiManager.healthBar.SetAmount(hp,maxHP);
            uiManager.manaBar.SetAmount(mp,maxMP);
            infoText.text = "Level up!";
            Instantiate(infoText, transform.position, Quaternion.identity);

            exp += extraXP;
        }
    }

    private IEnumerator NextBattle()
    {
        yield return new WaitForSeconds(battleDelayTime);
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
