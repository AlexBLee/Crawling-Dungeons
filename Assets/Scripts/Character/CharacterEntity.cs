using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

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

    // Conditions --------------------------
    public bool inBattle;
    protected bool dead = false;

    // Others ----------------------------
    
    [HideInInspector] public Animator anim;
    protected Vector3 initialPos;
    public CharacterEntity target;
    [HideInInspector] public TextMeshPro infoText;
    public Spell spellUsed;


    // ------------------------------------

    #region Stats  

    public void InitalizeStats()
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

    protected void LevelUp()
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

    protected void UpdateDamageStats()
    {
        minDamage = (str.amount / PlayerDefaultConstants.MinDamageStrCalc);
        maxDamage = (str.amount / PlayerDefaultConstants.MaxDamageStrCalc);

        minDamage += (dex.amount / PlayerDefaultConstants.MinDamageDexCalc);
        maxDamage += (dex.amount / PlayerDefaultConstants.MinDamageStrCalc);

        magicMinDamage = (intl.amount / PlayerDefaultConstants.MagicMinDamageCalc);
        magicMaxDamage = (intl.amount / PlayerDefaultConstants.MagicMaxDamageCalc);

        critChance = (luck.amount / PlayerDefaultConstants.CritChanceCalc);
        dodgeChance = (dex.amount / PlayerDefaultConstants.DodgeChanceCalc);

        def = (str.amount / PlayerDefaultConstants.DefCalc);
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

        // TODO reorganize this whole function
        int randomMin = 0;
        int randomMax = 100;
        float defStartPointCalc = 1f;
        float convertToPercentageCalc = 100;
        int critBonusDamage = 2;

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

        damage += additionalDamage;

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

        additionalDamage = 0;
    }

    // Currently a seperate funciton due to magic possibly not being just a basic attack?
    protected void DoMagicDamage()
    {
        // Damage is calculated by finding a random value between the minimum/maximum damage, and then taking the damage reduced by the enemy's defense.
        // for clarification, 1 def is equal to 1% of damage reduced.
        // so in this case, the enemy takes 99% of the damage.

        // TODO reorganize this whole function
        int randomMin = 0;
        int randomMax = 100;
        float defStartPointCalc = 1f;
        float convertToPercentageCalc = 100;
        int critBonusDamage = 2;

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

        float modifiedTargetPos = targetPosition.x - transform.right.x;
        transform.DOMoveX(modifiedTargetPos, 1).OnComplete(StartAttack);
    }

    private void StartAttack()
    {
        anim.SetBool("Run", false);
        anim.SetTrigger("Attack");
    }

    // Animation Finishes - triggered in animations.
    public void AnimationFinish()
    {
        transform.DOMove(initialPos, 1);
        ToggleNextTurn();
    }

    public void MagicAnimationFinish()
    {
        ToggleNextTurn();
    }

    #endregion

    public void PlaySound(string n)
    {
        AudioManager.Instance.Play(n);
    }

    private void ToggleNextTurn()
    {
        StartCoroutine(battleManager.ToggleNextTurn());
    }
}
