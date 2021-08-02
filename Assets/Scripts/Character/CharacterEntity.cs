using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    // Stat Calculating ----------------------------------
    private const int initial_level = 1;
    private const int initial_exp = 0;
    private const int initial_expThreshold = 50;
    private const int initial_statPoints = 3;
    private const int initial_maxHP = 100;
    private const int initial_maxMP = 100;
    private const int initial_stat_amount = 20;

    private const int growth_expThreshold = 200;
    private const int growth_statPoints = 3;
    private const int growth_hpGrowth = 10;
    private const int growth_mpGrowth = 10;
    private const int growth_statAmount = 3;

    private const int minDamageStrCalc = 3;
    private const float maxDamageStrCalc = 2.5f;
    private const float minDamageDexCalc = 3.5f;
    private const float maxDamageDexCalc = 4.5f;
    private const int magicMinDamageCalc = 5;
    private const int magicMaxDamageCalc = 2;
    private const int critChanceCalc = 2;
    private const int dodgeChanceCalc = 8;
    private const int defCalc = 10;

    private const float defStartPointCalc = 1.00f;
    private const float convertToPercentageCalc = 100;

    private const int critBonusDamage = 2;

    private const int randomMin = 0;
    private const int randomMax = 100;

    private const float approachDistance = 1.5f;
    private const float approachSpeed = 5;
    private const float timeApproachSpeed = 1.20f;

    

    // Conditions --------------------------
    public bool inBattle;
    protected bool dead = false;

    // Others ----------------------------
    
    [HideInInspector] public Animator anim;
    protected Vector3 initialPos;
    public CharacterEntity target;
    [HideInInspector] public TextMeshPro infoText;
    public SpellNew spellUsed;


    // ------------------------------------

    #region Stats  

    public void InitalizeStats()
    {
        level = initial_level;
        exp = initial_exp;
        expThreshold = initial_expThreshold;
        statPoints = initial_statPoints;

        maxHP = initial_maxHP;
        maxMP = initial_maxMP;

        hp = maxHP;
        mp = maxMP;

        str.amount = initial_stat_amount;
        intl.amount = initial_stat_amount;
        dex.amount = initial_stat_amount;
        luck.amount = initial_stat_amount;

        UpdateDamageStats();
    }

    protected void LevelUp()
    {
        level++;
        exp = initial_exp;
        expThreshold += growth_expThreshold;
        statPoints += growth_statPoints;

        hp += growth_hpGrowth;
        mp += growth_mpGrowth;

        maxHP += growth_hpGrowth;
        maxMP += growth_mpGrowth;

        str.amount += growth_statAmount;
        intl.amount += growth_statAmount;
        dex.amount += growth_statAmount;
        luck.amount += growth_statAmount;

        UpdateDamageStats();
    }

    protected void UpdateDamageStats()
    {
        minDamage = str.amount / minDamageStrCalc;
        maxDamage = str.amount / maxDamageStrCalc;

        minDamage += dex.amount / minDamageDexCalc;
        maxDamage += dex.amount / maxDamageDexCalc;

        magicMinDamage = intl.amount / magicMinDamageCalc;
        magicMaxDamage = intl.amount / magicMaxDamageCalc;

        critChance = luck.amount / critChanceCalc;
        dodgeChance = dex.amount / dodgeChanceCalc;

        def = str.amount / defCalc;
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

    private void CheckDeath()
    {
        if(hp <= 0 && inBattle)
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
    
    public void DoDamage()
    {
        // Damage is calculated by finding a random value between the minimum/maximum damage, and then taking the damage reduced by the enemy's defense.
        // for clarification, 1 def is equal to 1% of damage reduced.
        // so in this case, the enemy takes 99% of the damage.

        float missChance = Random.Range(randomMin,randomMax);
        if (missChance < target.dodgeChance)
        {
            infoText.text = "Miss!";
            AudioManager.Instance.Play("Woosh");
            Instantiate(infoText, target.transform.position, Quaternion.identity);
            return;
        }

        // Initial damage calculation
        float x = Random.Range(minDamage,maxDamage);
        x *= (defStartPointCalc - ((float)target.def/convertToPercentageCalc)); 
        int damage = Mathf.RoundToInt(x);

        // Critcal chance calculation
        float chance = Random.Range(randomMin, randomMax);
        if(chance < critChance)
        {
            damage *= critBonusDamage;
            
            if(target is Player)
            {
                LeftHand shield = (LeftHand)target.GetComponent<Inventory>().equips[3];
                if(shield != null)
                {
                    damage = shield.ReduceCrit(damage);
                }
            }
            
            infoText.color = Color.yellow;
        }
        else
        {
            infoText.color = Color.white;
        }

        // Apply damage
        target.hp -= damage;
        target.anim.SetTrigger("Hit");
        AudioManager.Instance.Play("SlashHit");
        
        // Spawning text
        infoText.text = damage.ToString();
        Instantiate(infoText, target.transform.position, Quaternion.identity);
        Debug.Log(gameObject.name + " dealt " + damage + " damage to " + target.name);

        if(target is Enemy)
        {
            target.CheckDeath();
        }
        else
        {
            uiManager.UpdateUIHealth();
            target.CheckDeath();
        }


    }

    // Currently a seperate funciton due to magic possibly not being just a basic attack?
    protected void DoMagicDamage()
    {
        // Damage is calculated by finding a random value between the minimum/maximum damage, and then taking the damage reduced by the enemy's defense.
        // for clarification, 1 def is equal to 1% of damage reduced.
        // so in this case, the enemy takes 99% of the damage.

        float missChance = Random.Range(randomMin,randomMax);
        if (missChance < target.dodgeChance)
        {
            infoText.text = "Miss!";
            AudioManager.Instance.Play("Woosh");
            Instantiate(infoText, target.transform.position, Quaternion.identity);
            return;
        }
        
        // Initial Damage Calculation
        float x = Random.Range(magicMinDamage,magicMaxDamage);
        x *= (defStartPointCalc - ((float)target.def/convertToPercentageCalc));
        int damage = Mathf.RoundToInt(x);
        damage += spellUsed.Damage;

        // Critical chance calculation
        float chance = Random.Range(randomMin, randomMax);
        if(chance < critChance)
        {
            damage *= critBonusDamage;
            
            if(target is Player)
            {
                LeftHand shield = (LeftHand)target.GetComponent<Inventory>().equips[3];
                if(shield != null)
                {
                    damage = shield.ReduceCrit(damage);
                }
            }
            
            infoText.color = Color.yellow;
        }
        else
        {
            infoText.color = Color.white;
        }

        // Apply damage
        target.hp -= damage;
        spellUsed.ApplyDebuffs(target);
        target.anim.SetTrigger("Hit");
        AudioManager.Instance.Play("SlashHit");

        // Spawn text
        infoText.text = damage.ToString();
        Instantiate(infoText, target.transform.position, Quaternion.identity);
        Debug.Log(gameObject.name + " dealt " + damage + " damage to " + target.name);

        if(target is Enemy)
        {
            target.GetComponent<Enemy>().CheckDeath();
        }
        else
        {
            uiManager.UpdateUIHealth();
            target.GetComponent<Player>().CheckDeath();
        }
        spellUsed = null;
    }
 
    // Next turn setting + damage is done through animation
    protected void RangedAttack()
    {
        uiManager.DisableButtons();
        anim.SetTrigger("Cast");
    }

    public void Heal(int amount, bool battleFinish)
    {
        if(this is Player)
        {
            uiManager.HidePotionList();
            uiManager.DisableButtons();
        }

        hp += amount;

        if(hp >= maxHP)
        {
            hp = maxHP;
        }

        if(!battleFinish)
        {
            infoText.text = amount.ToString();
            infoText.color = Color.green;
            Instantiate(infoText, transform.position, Quaternion.identity);
            AudioManager.Instance.Play("UsePotion");
            anim.SetTrigger("Heal");
        }

        uiManager.UpdateUIHealth();

    }

    public void RestoreMP(int amount, bool battleFinish)
    {
        if(this is Player)
        {
            uiManager.HidePotionList();
            uiManager.DisableButtons();
        }

        mp += amount;

        if(mp >= maxMP)
        {
            mp = maxMP;
        }

        if(!battleFinish)
        {
            infoText.text = amount.ToString();
            infoText.color = Color.cyan;
            Instantiate(infoText, transform.position, Quaternion.identity);
            AudioManager.Instance.Play("UsePotion");
            anim.SetTrigger("Heal");
        }

        uiManager.UpdateUIMana();
    }

    #endregion

    #region Animations/Movement
   
    protected void MoveToAttackPosition(Vector2 targetPosition)
    {
        anim.SetBool("Run", true);
        iTween.MoveTo(gameObject, iTween.Hash("x", targetPosition.x - approachDistance, "onComplete", "StartAttacking"));
    }

    void StartAttacking()
    {
        anim.SetBool("Run", false);
        anim.SetTrigger("Attack");
    }

    // Animation Finishes - triggered in animations.
    public void AnimationFinish()
    {
        iTween.MoveTo(gameObject, iTween.Hash("x", initialPos.x));
        battleManager.ToggleNextTurn();
    }

    public void MagicAnimationFinish()
    {
        StartCoroutine(battleManager.ToggleNextTurn());
    }

        
    public void PlaySound(string n)
    {
        AudioManager.Instance.Play(n);
    }
    #endregion
}
