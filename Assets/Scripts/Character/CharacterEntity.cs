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

    public int hp;
    public int mp;

    public int maxHP;
    public int maxMP;

    public int str;
    public int intl;
    public int dex;
    public int will;

    public int additionalDamage;
    public float critChance;

    public float minDamage;
    public float maxDamage;

    public float magicMinDamage;
    public float magicMaxDamage;

    public int def;

    public int moveSpeed = 5;


    // ---------------------------------------

    // Conditions --------------------------
    public bool inBattle;
    public bool targetReached = true;
    public bool attacking = false;
    public bool dead = false;
    public bool animationDone = false;

    // ------------------------------------


    // Others ----------------------------
    public Animator anim;
    protected Vector3 initialPos;
    public CharacterEntity target;
    public TextMeshPro infoText;
    public int objectID;
    public int hpCounter;
    public int mpCounter;


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

        str = 20;
        intl = 20;
        dex = 20;
        will = 20;

        UpdateDamageStats();
    }

    protected void LevelUp()
    {
        level++;
        exp = 0;
        expThreshold += 300;
        statPoints += 3;

        hp += 10;
        mp += 10;

        maxHP += 10;
        maxMP += 10;

        str += 3;
        intl += 3;
        dex += 3;
        will += 3;

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

        critChance = (will - 10)/10;

        def = str / 10;
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
        will = otherChar.will;

        objectID = otherChar.objectID;
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
        otherChar.will = will;

        otherChar.objectID = objectID;
        UpdateDamageStats();


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
            infoText.color = new Color(255,255,0);
        }

        // Apply damage
        target.hp -= damage;
        target.anim.SetTrigger("Hit");
        
        // Spawning text
        infoText.text = damage.ToString();
        Instantiate(infoText, target.transform.position + new Vector3(0,0,-1), Quaternion.identity);
        infoText.color = new Color(255,255,255);
        Debug.Log(gameObject.name + " dealt " + damage + " damage to " + target.name);

        if(target is Enemy)
        {
            target.GetComponent<Enemy>().CheckDeath();
        }
        else
        {
            uiManager.UpdateUIHealth();
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
        }

        // Apply damage
        target.hp -= damage;
        target.anim.SetTrigger("Hit");
        
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
        }

    }
 
    // Next turn setting + damage is done through animation
    protected void RangedAttack()
    {
        uiManager.DisableButtons();
        anim.SetTrigger("Cast");
        attacking = false;
    }

    public void UseHealthItem(ConsumableItem item)
    {
        if(hpCounter > 0)
        {
            uiManager.DisableButtons();
            uiManager.HidePotionList();

            int amountToHeal = Mathf.RoundToInt(maxHP * item.hpAdd);
            hp += amountToHeal;

            if(hp >= maxHP)
            {
                hp = maxHP;
            }
            
            infoText.text = amountToHeal.ToString();
            Instantiate(infoText, transform.position + new Vector3(0,0,-1), Quaternion.identity);
            anim.SetTrigger("UseItem");

            if(this is Player)
            {
                uiManager.UpdateUIHealth();

            }

            hpCounter--;
        }
    }

    public void UseManaItem(ConsumableItem item)
    {
        if(mpCounter > 0)
        {
            uiManager.DisableButtons();
            uiManager.HidePotionList();

            int amountToRestore = Mathf.RoundToInt(maxMP * item.mpAdd);
            mp += amountToRestore;

            if(mp >= maxMP)
            {
                mp = maxMP;
            }

            infoText.text = amountToRestore.ToString();
            Instantiate(infoText, transform.position + new Vector3(0,0,-1), Quaternion.identity);
            anim.SetTrigger("UseItem");

            if(this is Player)
            {
                uiManager.UpdateUIMana();
            }

            mpCounter--;
        }
    }

    #endregion

    #region Animations/Movement
   
    protected void MoveAndAttack(Vector2 targetPosition, int direction)
    {
        if(!targetReached && attacking)
        {
            if (Vector2.Distance(transform.position, targetPosition) > 1.0f)
            {
                transform.position += (transform.right * direction) * Time.deltaTime * moveSpeed;
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
                battleManager.ToggleNextTurn();
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
            transform.position = Vector2.Lerp(startPos,endPos, timer/2);
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
        battleManager.ToggleNextTurn();
    }

        
    public void PlaySound(string n)
    {
        AudioManager.Instance.Play(n);
    }
    #endregion
}
