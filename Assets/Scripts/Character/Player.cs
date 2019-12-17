using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Player : CharacterEntity
{
    public AmountBar healthBar;
    public AmountBar manaBar;

    public Vector3 pos;
    public Vector3 spellPosition;

    public Inventory inventory;

    // Equipped Items
    public List<Spell> spells;
    public Dictionary<Spell, bool> spellList;


    public AudioClip clip;



    private void Awake() 
    {
        inventory = GetComponent<Inventory>();
        ApplyStatsFrom(GameManager.instance.playerStats);

        spellList = new Dictionary<Spell, bool>();
        foreach(Spell spell in spells)
        {
            spellList.Add(spell, false);
        }

        uiManager.UpdateUIHealth();
        uiManager.UpdateUIMana();

    }


    private void Start()
    {

        pos = transform.position;

        UnlockSpells();
        UpdateDamageStats();
        inventory.UpdateItemStats();

    }

    private void Update()
    {
        // --------------------------------------------------------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------------------------------------------------------- //


        if(inBattle && battleManager.playerTurn)
        {
            if (target != null)
            {     
                MoveAndAttack(target.transform.position, 1);
            }
            StopAttacking();  
        }

        // --------------------------------------------------------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------------------------------------------------------- //

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
                Instantiate(spell.effect, spellPosition, Quaternion.identity);            
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
                will++;
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
                will--;
                break;
            default:
                break;
        }

        statPoints++;
        UpdateDamageStats();
    }

    public void RecieveXPAndGold(int expRecieved, int goldRecieved)
    {
        inventory.gold += goldRecieved;
        exp += expRecieved;
        infoText.text = "+" + expRecieved.ToString() + " XP";
        Instantiate(infoText, transform.position, Quaternion.identity);
        StartCoroutine(CheckForLevelUp());
        StartCoroutine(NextBattle());

    }

    public IEnumerator CheckForLevelUp()
    {
        while(exp >= expThreshold)
        {
            float extraXP = exp - expThreshold;
            LevelUp();
            UnlockSpells();
            inventory.UpdateItemStats();

            
            yield return new WaitForSeconds(0.15f);
            healthBar.SetAmount(hp,maxHP);
            manaBar.SetAmount(mp,maxMP);
            infoText.text = "Level up!";
            Instantiate(infoText, transform.position, Quaternion.identity);

            exp += extraXP;
        }

    }

    public void UnlockSpells()
    {

        if(level >= 1) { spellList[spells[0]] = true; }

        if(level >= 5) { spellList[spells[1]] = true; }

        if(level >= 7) { spellList[spells[2]] = true; }

        if(level >= 12) { spellList[spells[3]] = true; }

        if(level >= 20) { spellList[spells[4]] = true; }

        if(level >= 25) { spellList[spells[5]] = true; }

    }

    public IEnumerator NextBattle()
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
