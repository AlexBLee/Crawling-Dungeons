using System.Collections;
using UnityEngine;

public class Player : CharacterEntity
{
    [HideInInspector] public Inventory inventory;
    [HideInInspector] public Spells spells;
    public enum StatType { Str, Dex, Intl, Luck };
    private int originalNumberOfStatPoints = 0;

    private void Awake() 
    {
        inventory = GetComponent<Inventory>();
        spells = GetComponent<Spells>();

        ApplyStatsFrom(GameManager.instance.playerStats);
    }

    private void Start()
    {
        initialPos = transform.position;

        if (Managers.Instance.UI != null)
        {
            Managers.Instance.UI.StatusHUD[0].UpdateAllBars();
        }
        
        UpdateDamageStats();
        spells.UnlockSpells(level);
        inventory.UpdateItemStats();
    }

    // -----------------------------------------------------------------------------

    private void InitalizeStats()
    {
        level =         PlayerDefaultConstants.InitialLevel;
        exp =           PlayerDefaultConstants.InitialExp;
        expThreshold =  PlayerDefaultConstants.InitialExpThreshold;
        statPoints =    PlayerDefaultConstants.InitialStatPoints;

        maxHP =         PlayerDefaultConstants.InitialMaxHP;
        maxMP =         PlayerDefaultConstants.InitialMaxMP;

        hp = maxHP;
        mp = maxMP;

        str.amount =    PlayerDefaultConstants.InitialStatAmount;
        intl.amount =   PlayerDefaultConstants.InitialStatAmount;
        dex.amount =    PlayerDefaultConstants.InitialStatAmount;
        luck.amount =   PlayerDefaultConstants.InitialStatAmount;

        UpdateDamageStats();
    }

    private void LevelUp()
    {
        level++;
        exp = 0;
        expThreshold += PlayerDefaultConstants.ExpThresholdGrowth;
        statPoints +=   PlayerDefaultConstants.StatPointsGrowth;

        hp += PlayerDefaultConstants.HpGrowth;
        mp += PlayerDefaultConstants.MpGrowth;

        maxHP += PlayerDefaultConstants.HpGrowth;
        maxMP += PlayerDefaultConstants.MpGrowth;

        str.amount += PlayerDefaultConstants.StatAmountGrowth;
        intl.amount += PlayerDefaultConstants.StatAmountGrowth;
        dex.amount += PlayerDefaultConstants.StatAmountGrowth;
        luck.amount += PlayerDefaultConstants.StatAmountGrowth;

        UpdateDamageStats();
    }
    
    public void ApplyStatsFrom(CharacterEntity otherChar)
    {
        level = otherChar.level;
        exp = otherChar.exp;
        expThreshold = otherChar.expThreshold;
        statPoints = otherChar.statPoints;

        hp = otherChar.hp;
        mp = otherChar.mp;

        maxHP = otherChar.maxHP;
        maxMP = otherChar.maxMP;

        str = otherChar.str;
        intl = otherChar.intl;
        dex = otherChar.dex;
        luck = otherChar.luck;

        UpdateDamageStats();

    }

    public void ApplyStatsTo(CharacterEntity otherChar)
    {
        otherChar.level = level;
        otherChar.exp = exp;
        otherChar.expThreshold = expThreshold;
        otherChar.statPoints = statPoints;

        otherChar.hp = hp;
        otherChar.mp = mp;

        otherChar.maxHP = maxHP;
        otherChar.maxMP = maxMP;

        otherChar.str = str;
        otherChar.intl = intl;
        otherChar.dex = dex;
        otherChar.luck = luck;

        UpdateDamageStats();
    }
    
    public void Attack()
    {
        Managers.Instance.UI.DisableButtons();
        MoveToAttackPosition(target.transform.position);
    }

    public bool HasEnoughManaForSpell(Spell spell)
    {
        return (spell.cost <= mp);
    }

    public void MagicPressed(Spell spell)
    {
        Color NotEnoughManaColor = new Color(0, 205, 255);

        if ((mp - spell.cost) < 0)
        {
            Managers.Instance.UI.SpawnInfoText(DisplayStrings.NotEnoughManaText, NotEnoughManaColor, transform.position);
        }
        else
        {
            AudioManager.Instance.PlaySound(spell.name);
            Managers.Instance.UI.HideMagicList();

            Debug.Log("Casted: " + spell.name);
            spellUsed = spell;
            mp -= spellUsed.cost;
            
            Managers.Instance.UI.StatusHUD[0].UpdateUIMana();

            RangedAttack();
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

    public void CheckStatAmount()
    {
        if (statPoints == 0)
        {
            Managers.Instance.UI.VictoryPanelHUD.ActivateModifiedSubtractors();
        }
        else if (statPoints == originalNumberOfStatPoints)
        {
            Managers.Instance.UI.VictoryPanelHUD.ActivateAddersOnly();
        }
        else if (statPoints > 0)
        {
            Managers.Instance.UI.VictoryPanelHUD.ActivateAllStatModifiers();
        }
    }

    #endregion

    public override void Heal(int amount, bool battleFinish)
    {
        Managers.Instance.UI.HidePotionList();
        Managers.Instance.UI.DisableButtons();

        base.Heal(amount, battleFinish);

        Managers.Instance.UI.StatusHUD[0].UpdateUIHealth();
    }

    public override void RestoreMP(int amount, bool battleFinish)
    {
        Managers.Instance.UI.HidePotionList();
        Managers.Instance.UI.DisableButtons();

        base.RestoreMP(amount, battleFinish);

        Managers.Instance.UI.StatusHUD[0].UpdateUIMana();
    }

    public void RecieveXPAndGold(int expRecieved, int goldRecieved)
    {
        const int GoldMinValue = 10;
        const int GoldMaxValue = 15;

        // Randomized gold - to vary playstyle
        int randomGold = Random.Range(goldRecieved - GoldMinValue, goldRecieved + GoldMaxValue);
        inventory.gold += randomGold;
        exp += expRecieved;

        Managers.Instance.UI.SpawnInfoText(string.Format(DisplayStrings.GainXPText, expRecieved), Color.white, transform.position);

        StartCoroutine(CheckForLevelUp());
        StartCoroutine(NextBattle());
    }

    private IEnumerator CheckForLevelUp()
    {
        const float LevelDelayTime = 0.15f;

        const float LevelDisplayOffset = 0.5f;
        Vector2 positionOffset = new Vector2(transform.position.x + LevelDisplayOffset, transform.position.y);

        while (exp >= expThreshold)
        {
            float extraXP = exp - expThreshold;
            LevelUp();
            spells.UnlockSpells(level);
            inventory.UpdateItemStats();

            yield return new WaitForSeconds(LevelDelayTime);
            Managers.Instance.UI.StatusHUD[0].UpdateUIHealth();
            Managers.Instance.UI.StatusHUD[0].UpdateUIMana();
            Managers.Instance.UI.MagicHUD.Init();
            
            Managers.Instance.UI.SpawnInfoText(DisplayStrings.LevelUpText, Color.white, positionOffset);

            exp += extraXP;
        }
    }

    private IEnumerator NextBattle()
    {
        const float BattleDelayTime = 3;

        yield return new WaitForSeconds(BattleDelayTime);
        anim.SetBool(CharacterClipAnims.RunAnimName, true);
        Managers.Instance.Battle.ToggleNextBattle();
    }

    public void DeclareVictory()
    {
        const float HpHeal = 0.15f;
        const float MpHeal = 0.15f;

        Heal((int)(maxHP * HpHeal), true);
        RestoreMP((int)(maxMP * MpHeal), true);

        originalNumberOfStatPoints = statPoints;
        CheckStatAmount();
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    public override void FinishDeath()
    {
        Managers.Instance.UI.ShowGameOver();
        base.FinishDeath();
    }

    public void Reset()
    {
        InitalizeStats();
        inventory.InitializeInventory();
    }
    


    // -----------------------------------------------------------------------------

}
