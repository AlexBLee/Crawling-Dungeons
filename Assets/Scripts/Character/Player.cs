using System.Collections;
using UnityEngine;

public class Player : CharacterEntity
{
    [HideInInspector] public Inventory inventory;
    [HideInInspector] public Spells spells;

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
                MoveAndAttack(target.transform.position, 1);
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

    public void AddToStat(int number)
    {
        switch (number)
        {
            case 1:
                str++;
                break;
            case 2:
                dex++;
                break;
            case 3:
                intl++;
                break;
            case 4:
                luck++;
                break;
            default:
                break;
        }

        statPoints--;
        UpdateDamageStats();
    }

    public void RemoveFromStat(int number)
    {
        switch (number)
        {
            case 1:
                str--;
                break;
            case 2:
                dex--;
                break;
            case 3:
                intl--;
                break;
            case 4:
                luck--;
                break;
            default:
                break;
        }

        statPoints++;
        UpdateDamageStats();
    }

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

    #endregion

    // -----------------------------------------------------------------------------

}
