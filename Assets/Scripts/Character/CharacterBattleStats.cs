using UnityEngine;

public class CharacterBattleStats : MonoBehaviour
{
    private int _hp;
    private int _mp;

    private int _maxHP;
    private int _maxMP;

    private Stat _str;
    private Stat _int;
    private Stat _dex;
    private Stat _luck;

    private float _minDamage;
    private float _maxDamage;
    
    private float _magicMinDamage;
    private float _magicMaxDamage;

    private int _def;
    
    private int _additionalDamage;
    private float _critChance;
    private float _dodgeChance;

    public float MinDamage => _minDamage;
    public float MaxDamage => _maxDamage;
    public float MagicMinDamage => _magicMinDamage;
    public float MagicMaxDamage => _magicMaxDamage;
    public float AdditionalDamage => _additionalDamage;
    public float CritChance => _critChance;
    public float DodgeChance => _dodgeChance;
    
    private void UpdateDamageStats()
    {
        _minDamage = _str.amount / PlayerDefaultConstants.MinDamageStrCalc;
        _maxDamage = _str.amount / PlayerDefaultConstants.MaxDamageStrCalc;

        _minDamage += _dex.amount / PlayerDefaultConstants.MinDamageDexCalc;
        _maxDamage += _dex.amount / PlayerDefaultConstants.MaxDamageDexCalc;

        _magicMinDamage = _int.amount / PlayerDefaultConstants.MagicMinDamageCalc;
        _magicMaxDamage = _int.amount / PlayerDefaultConstants.MagicMaxDamageCalc;

        _critChance = _luck.amount / PlayerDefaultConstants.CritChanceCalc;
        _dodgeChance = _dex.amount / PlayerDefaultConstants.DodgeChanceCalc;

        _def = _str.amount / PlayerDefaultConstants.DefCalc;
    }

    public void RemoveHealth(int amount)
    {
        _hp -= amount;
    }
    
    public void Heal(int amount, bool battleFinish)
    {
        _hp += amount;

        if (_hp >= _maxHP)
        {
            _hp = _maxHP;
        }

        if (battleFinish)
        {
            return;
        }
        
        SpawnRestoringUI(amount.ToString(), Color.green);
    }

    public void RestoreMp(int amount, bool battleFinish)
    {
        _mp += amount;

        if (_mp >= _maxMP)
        {
            _mp = _maxMP;
        }

        if (battleFinish)
        {
            return;
        }
        
        SpawnRestoringUI(amount.ToString(), Color.cyan);
    }

    private void SpawnRestoringUI(string amountString, Color color)
    {
        Managers.Instance.UI.SpawnInfoText(amountString, Color.cyan, transform.position);
        AudioManager.Instance.PlaySound(AudioStrings.UsePotion);
        // anim.SetTrigger(CharacterClipAnims.HealAnimName);
    }
    
    public void CheckDeath()
    {
        if (_hp <= 0) // && inBattle)
        {
            // dead = true;
            // spellModifiers.Clear();
            // anim.SetTrigger(CharacterClipAnims.DeathAnimName);
        }
    }
    
    
}
