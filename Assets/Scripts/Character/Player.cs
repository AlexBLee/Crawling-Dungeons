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

        if(uiManager != null)
        {
            uiManager.StatusHUD.UpdateAllBars();
        }

        UpdateDamageStats();
        spells.UnlockSpells(level);
        inventory.UpdateItemStats();
    }

    // -----------------------------------------------------------------------------

    public void Attack()
    {
        uiManager.DisableButtons();
        MoveToAttackPosition(target.transform.position);
    }

    public bool HasEnoughManaForSpell(Spell spell)
    {
        return (spell.cost <= mp);
    }

    public void MagicPressed(Spell spell)
    {
        Color NotEnoughManaColor = new Color(0, 205, 255);

        if((mp - spell.cost) < 0)
        {
            Debug.Log("not enough mana!");
            infoText.color = NotEnoughManaColor;
            infoText.text = "Not enough mana!";
            Instantiate(infoText, transform.position, Quaternion.identity);
            infoText.color = Color.white;
        }
        else
        {
            AudioManager.Instance.PlaySound(spell.name);
            uiManager.HideMagicList();

            Debug.Log("Casted: " + spell.name);
            spellUsed = spell;
            mp -= spellUsed.cost;
            
            uiManager.StatusHUD.UpdateUIMana();

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
            uiManager.VictoryPanelHUD.ActivateModifiedSubtractors();
        }
        else if (statPoints == originalNumberOfStatPoints)
        {
            uiManager.VictoryPanelHUD.ActivateAddersOnly();
        }
        else if (statPoints > 0)
        {
            uiManager.VictoryPanelHUD.ActivateAllStatModifiers();
        }
    }

    #endregion


    public void RecieveXPAndGold(int expRecieved, int goldRecieved)
    {
        const int GoldMinValue = 10;
        const int GoldMaxValue = 15;

        // Randomized gold - to vary playstyle
        int randomGold = Random.Range(goldRecieved - GoldMinValue, goldRecieved + GoldMaxValue);
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
        const float LevelDelayTime = 0.15f;

        while (exp >= expThreshold)
        {
            float extraXP = exp - expThreshold;
            LevelUp();
            spells.UnlockSpells(level);
            inventory.UpdateItemStats();

            yield return new WaitForSeconds(LevelDelayTime);
            uiManager.StatusHUD.UpdateUIHealth();
            uiManager.StatusHUD.UpdateUIMana();
            uiManager.MagicHUD.Init();
            infoText.text = "Level up!";
            Instantiate(infoText, transform.position, Quaternion.identity);

            exp += extraXP;
        }
    }

    private IEnumerator NextBattle()
    {
        const float BattleDelayTime = 3;

        yield return new WaitForSeconds(BattleDelayTime);
        anim.SetBool("Run", true);
        battleManager.battleDone = true;
        battleManager.ToggleNextBattle();
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

    public override void FinishDeath()
    {
        uiManager.ShowGameOver();
        base.FinishDeath();
    }


    // -----------------------------------------------------------------------------

}
