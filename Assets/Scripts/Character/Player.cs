using System.Collections;
using UnityEngine;

public class Player : CharacterEntity
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Spells _spells;

    public Inventory Inventory => _inventory;
    public Spells Spells => _spells;

    private void Awake() 
    {
        CharacterBattleStats.ApplyStatsFrom(GameManager.instance.playerStats.CharacterBattleStats);
    }

    private void Start()
    {
        initialPos = transform.position;

        if (Managers.Instance.UI != null)
        {
            Managers.Instance.UI.StatusHUD[0].UpdateAllBars();
        }
        
        _characterBattleStats.UpdateDamageStats();
        _spells.UnlockSpells(_characterBattleStats._level);
        _inventory.UpdateItemStats();
    }

    // -----------------------------------------------------------------------------

    public void Attack()
    {
        Managers.Instance.UI.DisableButtons();
        MoveToAttackPosition(target.transform.position);
    }

    public bool HasEnoughManaForSpell(Spell spell)
    {
        return (spell.Cost <= _characterBattleStats.Mp);
    }

    public void MagicPressed(Spell spell)
    {
        Color notEnoughManaColor = new Color(0, 205, 255);

        if (_characterBattleStats.Mp - spell.Cost < 0)
        {
            Managers.Instance.UI.SpawnInfoText(DisplayStrings.NotEnoughManaText, notEnoughManaColor, transform.position);
        }
        else
        {
            AudioManager.Instance.PlaySound(spell.Name);
            Managers.Instance.UI.HideMagicList();

            Debug.Log("Casted: " + spell.Name);
            spellUsed = spell;
            _characterBattleStats.Mp -= spellUsed.Cost;
            
            Managers.Instance.UI.StatusHUD[0].UpdateUIMana();

            _damageDealer.RangedAttack();
        }
    }

    private void Heal(int amount, bool battleFinish)
    {
        Managers.Instance.UI.HidePotionList();
        Managers.Instance.UI.DisableButtons();
        
        _characterBattleStats.Heal(amount, battleFinish);

        Managers.Instance.UI.StatusHUD[0].UpdateUIHealth();
    }

    private void RestoreMP(int amount, bool battleFinish)
    {
        Managers.Instance.UI.HidePotionList();
        Managers.Instance.UI.DisableButtons();

        _characterBattleStats.Heal(amount, battleFinish);

        Managers.Instance.UI.StatusHUD[0].UpdateUIMana();
    }

    public void RecieveXPAndGold(int expReceived, int goldReceived)
    {
        const int GoldMinValue = 10;
        const int GoldMaxValue = 15;

        // Randomized gold - to vary playstyle
        var randomGold = Random.Range(goldReceived - GoldMinValue, goldReceived + GoldMaxValue);
        _inventory.gold += randomGold;
        _characterBattleStats._exp += expReceived;

        Managers.Instance.UI.SpawnInfoText(string.Format(DisplayStrings.GainXPText, expReceived), Color.white, transform.position);

        StartCoroutine(CheckForLevelUp());
        StartCoroutine(NextBattle());
    }

    private IEnumerator CheckForLevelUp()
    {
        const float LevelDelayTime = 0.15f;

        const float LevelDisplayOffset = 0.5f;
        Vector2 positionOffset = new Vector2(transform.position.x + LevelDisplayOffset, transform.position.y);

        while (_characterBattleStats._exp >= _characterBattleStats._expThreshold)
        {
            var extraXP = _characterBattleStats._exp - _characterBattleStats._expThreshold;
            _characterBattleStats.LevelUp();
            _spells.UnlockSpells(_characterBattleStats._level);
            _inventory.UpdateItemStats();

            yield return new WaitForSeconds(LevelDelayTime);
            Managers.Instance.UI.StatusHUD[0].UpdateUIHealth();
            Managers.Instance.UI.StatusHUD[0].UpdateUIMana();
            Managers.Instance.UI.MagicHUD.Init();
            
            Managers.Instance.UI.SpawnInfoText(DisplayStrings.LevelUpText, Color.white, positionOffset);

            _characterBattleStats._exp += extraXP;
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

        Heal((int)(_characterBattleStats.MaxHp * HpHeal), true);
        RestoreMP((int)(_characterBattleStats.MaxMp * MpHeal), true);

        _originalNumberOfStatPoints = _statPoints;
        CheckStatAmount();
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }

    public override void FinishDeath()
    {
        Managers.Instance.UI.ShowGameOver();
        base.FinishDeath();
    }

    public void Reset()
    {
        _characterBattleStats.InitializeStats();
        _inventory.InitializeInventory();
    }
    


    // -----------------------------------------------------------------------------

}
