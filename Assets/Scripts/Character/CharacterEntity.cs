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
    public int str, intl, dex, luck;

    protected int additionalDamage;
    [SerializeField] protected float critChance;

    public float minDamage, maxDamage;
    public float magicMinDamage, magicMaxDamage;

    public int def;

    // Conditions --------------------------
    public bool inBattle;
    protected bool targetReached = true;
    protected bool attacking = false;
    protected bool dead = false;
    protected bool animationDone = false;

    // Others ----------------------------
    
    [HideInInspector] public Animator anim;
    protected Vector3 initialPos;
    public CharacterEntity target;
    [HideInInspector] public TextMeshPro infoText;


    // ------------------------------------

    #region Stats  

    public void InitalizeStats()
    {
        level = 1;
        exp = 0;
        expThreshold = 50;
        statPoints = 0;

        hp = 100;
        mp = 100;

        maxHP = 100;
        maxMP = 100;

        str = 20 + Random.Range(-2,2);
        intl = 20 + Random.Range(-2,2);
        dex = 20 + Random.Range(-2,2);
        luck = 20 + Random.Range(-2,2);

        UpdateDamageStats();
    }

    protected void LevelUp()
    {
        level++;
        exp = 0;
        expThreshold += 200;
        statPoints += 3;

        hp += 10;
        mp += 10;

        maxHP += 10;
        maxMP += 10;

        str += 3;
        intl += 3;
        dex += 3;
        luck += 3;

        UpdateDamageStats();
    }

    protected void UpdateDamageStats()
    {
        minDamage = str / 3;
        maxDamage = str / 2.5f;

        minDamage += dex / 3.5f;
        maxDamage += dex / 4.5f;

        magicMinDamage = intl / 5;
        magicMaxDamage = intl / 2;

        critChance = luck / 2;

        def = dex / 10;
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
    
    protected void DoDamage()
    {
        // Damage is calculated by finding a random value between the minimum/maximum damage, and then taking the damage reduced by the enemy's defense.
        // for clarification, 1 def is equal to 1% of damage reduced.
        // so in this case, the enemy takes 99% of the damage.
        
        // Initial damage calculation
        float x = Random.Range(minDamage,maxDamage);
        x *= (1.00f - ((float)target.def/100));
        int damage = Mathf.RoundToInt(x);

        // Critcal chance calculation
        float chance = Random.Range(0, 100);
        if(chance < critChance)
        {
            damage *= 2;
            
            if(target is Player)
            {
                LeftHand shield = (LeftHand)target.GetComponent<Inventory>().equips[3];
                if(shield != null)
                {
                    damage = shield.ReduceCrit(damage);
                }
            }
            
            infoText.color = new Color(255,255,0);
        }
        else
        {
            infoText.color = new Color(255,255,255);
        }

        // Apply damage
        target.hp -= damage;
        target.anim.SetTrigger("Hit");
        AudioManager.Instance.Play("SlashHit");
        
        // Spawning text
        infoText.text = damage.ToString();
        Instantiate(infoText, target.transform.position + new Vector3(0,0,-1), Quaternion.identity);
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
        
        // Initial Damage Calculation
        float x = Random.Range(magicMinDamage,magicMaxDamage);
        x *= (1.00f - ((float)target.def/100));
        int damage = Mathf.RoundToInt(x);
        damage += additionalDamage;

        // Critical chance calculation
        float chance = Random.Range(0, 100);
        if(chance < critChance)
        {
            damage *= 2;
            
            if(target is Player)
            {
                LeftHand shield = (LeftHand)target.GetComponent<Inventory>().equips[3];
                if(shield != null)
                {
                    damage = shield.ReduceCrit(damage);
                }
            }
            
            infoText.color = new Color(255,255,0);
        }
        else
        {
            infoText.color = new Color(255,255,255);
        }

        // Apply damage
        target.hp -= damage;
        target.anim.SetTrigger("Hit");
        AudioManager.Instance.Play("SlashHit");

        // Spawn text
        infoText.text = damage.ToString();
        Instantiate(infoText, target.transform.position + new Vector3(0,0,-1), Quaternion.identity);
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

    }
 
    // Next turn setting + damage is done through animation
    protected void RangedAttack()
    {
        uiManager.DisableButtons();
        anim.SetTrigger("Cast");
        attacking = false;
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
            infoText.color = new Color(0,255,0);
            Instantiate(infoText, transform.position + new Vector3(0,0,-1), Quaternion.identity);
            AudioManager.Instance.Play("UsePotion");
            anim.SetTrigger("UseItem");
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
            infoText.color = new Color(0,255,0);
            Instantiate(infoText, transform.position + new Vector3(0,0,-1), Quaternion.identity);
            AudioManager.Instance.Play("UsePotion");
            anim.SetTrigger("UseItem");
        }

        uiManager.UpdateUIMana();
    }

    #endregion

    #region Animations/Movement
   
    protected void MoveAndAttack(Vector2 targetPosition, int direction)
    {
        if(!targetReached && attacking)
        {
            if (Vector2.Distance(transform.position, targetPosition) > 1.0f)
            {
                transform.position += (transform.right * direction) * Time.deltaTime * 5;
                anim.SetBool("Run", true);
            }
            else
            {
                anim.SetBool("Run", false);
                anim.SetTrigger("Attack");
                targetReached = true;
            }

        }
    }

    protected void StopAttacking()
    {
        if(targetReached && attacking && animationDone)
        {
            StartCoroutine(MoveToExactPosition(initialPos));
            
            if(transform.position == initialPos)
            {
                StartCoroutine(battleManager.ToggleNextTurn());
                targetReached = false;
                attacking = false;
                animationDone = false;                   
            }

        }
    }

    protected IEnumerator MoveToExactPosition(Vector2 destination)
    {
        Vector2 startPos = transform.position;
        Vector2 endPos = destination;

        float timer = 0;
        while(timer < 2)
        {
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos,endPos, timer/1.20f);
            yield return null;
        }

    }

    // Animation Finishes - triggered in animations.
    public void AnimationFinish()
    {
        animationDone = true;
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
