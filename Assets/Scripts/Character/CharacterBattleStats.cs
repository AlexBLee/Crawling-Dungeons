using UnityEngine;

public class CharacterBattleStats : MonoBehaviour
{
    [SerializeField] private AnimationPlayer _animationPlayer;
    
    public int _level;
    public float _exp;
    public float _expThreshold;
    public int _statPoints;
    
   public int Hp;
   public int Mp;

   public int MaxHp;
   public int MaxMp;

   public Stat Str;
   public Stat Int;
   public Stat Dex;
   public Stat Luck;

   public float MinDamage;
   public float MaxDamage;
   public float MagicMinDamage;
   public float MagicMaxDamage;

   public int Def;
   public int AdditionalDamage;
   public float CritChance;
   public float DodgeChance;
    
    public enum StatType { Str, Dex, Intl, Luck };
    
    public void DeserializeEnemyData(EnemyData data)
    {
        MaxHp = data.maxHP;
        Hp = MaxHp;

        MaxMp = data.maxMP;
        Mp = MaxMp;

        Str.amount = data.str;
        Int.amount = data.intl;
        Dex.amount = data.dex;
        Luck.amount = data.luck;

        Def = data.def;
    }
    
    public void InitializeStats()
    {
        _level =         PlayerDefaultConstants.InitialLevel;
        _exp =           PlayerDefaultConstants.InitialExp;
        _expThreshold =  PlayerDefaultConstants.InitialExpThreshold;
        _statPoints =    PlayerDefaultConstants.InitialStatPoints;
        
        MaxHp =         PlayerDefaultConstants.InitialMaxHP;
        MaxMp =         PlayerDefaultConstants.InitialMaxMP;

        Hp = MaxHp;
        Mp = MaxMp;

        Str.amount =    PlayerDefaultConstants.InitialStatAmount;
        Int.amount =   PlayerDefaultConstants.InitialStatAmount;
        Dex.amount =    PlayerDefaultConstants.InitialStatAmount;
        Luck.amount =   PlayerDefaultConstants.InitialStatAmount;

        UpdateDamageStats();
    }

    public void LevelUp()
    {
        _level++;
        _exp = 0;
        _expThreshold += PlayerDefaultConstants.ExpThresholdGrowth;
        _statPoints +=   PlayerDefaultConstants.StatPointsGrowth;
        
        Hp += PlayerDefaultConstants.HpGrowth;
        Mp += PlayerDefaultConstants.MpGrowth;

        MaxHp += PlayerDefaultConstants.HpGrowth;
        MaxMp += PlayerDefaultConstants.MpGrowth;

        Str.amount += PlayerDefaultConstants.StatAmountGrowth;
        Int.amount += PlayerDefaultConstants.StatAmountGrowth;
        Dex.amount += PlayerDefaultConstants.StatAmountGrowth;
        Luck.amount += PlayerDefaultConstants.StatAmountGrowth;

        UpdateDamageStats();
    }
    
    public void ApplyStatsFrom(CharacterBattleStats otherStats)
    {
        _level = otherStats._level;
        _exp = otherStats._exp;
        _expThreshold = otherStats._expThreshold;
        _statPoints = otherStats._statPoints;
        
        Hp = otherStats.Hp;
        Mp = otherStats.Mp;

        MaxHp = otherStats.MaxHp;
        MaxMp = otherStats.MaxMp;

        Str = otherStats.Str;
        Int = otherStats.Int;
        Dex = otherStats.Dex;
        Luck = otherStats.Luck;

        UpdateDamageStats();
    }

    public void ApplyStatsTo(CharacterBattleStats otherStats)
    {
        otherStats._level = _level;
        otherStats._exp = _exp;
        otherStats._expThreshold = _expThreshold;
        otherStats._statPoints = _statPoints;
        
        otherStats.Hp = Hp;
        otherStats.Mp = Mp;

        otherStats.MaxHp = MaxHp;
        otherStats.MaxMp = MaxMp;

        otherStats.Str = Str;
        otherStats.Int = Int;
        otherStats.Dex = Dex;
        otherStats.Luck = Luck;

        UpdateDamageStats();
    }
    
    public void UpdateDamageStats()
    {
        MinDamage = Str.amount / PlayerDefaultConstants.MinDamageStrCalc;
        MaxDamage = Str.amount / PlayerDefaultConstants.MaxDamageStrCalc;

        MinDamage += Dex.amount / PlayerDefaultConstants.MinDamageDexCalc;
        MaxDamage += Dex.amount / PlayerDefaultConstants.MaxDamageDexCalc;

        MagicMinDamage = Int.amount / PlayerDefaultConstants.MagicMinDamageCalc;
        MagicMaxDamage = Int.amount / PlayerDefaultConstants.MagicMaxDamageCalc;

        CritChance = Luck.amount / PlayerDefaultConstants.CritChanceCalc;
        DodgeChance = Dex.amount / PlayerDefaultConstants.DodgeChanceCalc;

        Def = Str.amount / PlayerDefaultConstants.DefCalc;
    }

    public bool AllocatePoints(StatType type)
    {
        switch (type)
        {
            case StatType.Str:
                Str = AddStat(Str);
                break;
            case StatType.Dex:
                Dex = AddStat(Dex);
                break;
            case StatType.Intl:
                Int = AddStat(Int);
                break;
            case StatType.Luck:
                Luck = AddStat(Luck);
                break;
        }

        _statPoints--;
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
                Str = RemoveStat(Str, statZero);
                statZero = Str.modified;
                break;
            case StatType.Dex:
                Dex = RemoveStat(Dex, statZero);
                statZero = Dex.modified;
                break;
            case StatType.Intl:
                Int = RemoveStat(Int, statZero);
                statZero = Int.modified;
                break;
            case StatType.Luck:
                Luck = RemoveStat(Luck, statZero);
                statZero = Luck.modified;
                break;
            default:
                return false;
        }

        _statPoints++;
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

    public void RemoveHealth(int amount)
    {
        Hp -= amount;
    }
    
    public void Heal(int amount, bool battleFinish)
    {
        Hp += amount;

        if (Hp >= MaxHp)
        {
            Hp = MaxHp;
        }

        if (battleFinish)
        {
            return;
        }
        
        SpawnRestoringUI(amount.ToString(), Color.green);
    }

    public void RestoreMp(int amount, bool battleFinish)
    {
        Mp += amount;

        if (Mp >= MaxMp)
        {
            Mp = MaxMp;
        }

        if (battleFinish)
        {
            return;
        }
        
        SpawnRestoringUI(amount.ToString(), Color.cyan);
    }

    private void SpawnRestoringUI(string amountString, Color color)
    {
        Managers.Instance.UI.SpawnInfoText(amountString, color, transform.position);
        AudioManager.Instance.PlaySound(AudioStrings.UsePotion);
        _animationPlayer.PlayAnimationTrigger(CharacterClipAnims.HealAnimName);
    }
    
    public void CheckDeath()
    {
        if (Hp > 0)
        {
            return;
        }
        
        _animationPlayer.PlayAnimationTrigger(CharacterClipAnims.DeathAnimName);
    }
    
    
}
