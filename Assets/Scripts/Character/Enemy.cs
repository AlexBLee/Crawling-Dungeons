using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterEntity
{
    private GameObject player;
    public bool beginTurn;
    public float detectDistance = 10.0f;
    public int experiencePoints;
    public bool newBattle;
    public Vector3 fightPosition;

    private void Start() 
    {   
        initialPos = transform.position;
    }

    void Update()
    {
        if(newBattle)
        {
            MoveToPosition();
        }

        if(inBattle && !battleManager.playerTurn)
        {            
            MoveAndAttack(target.transform.position, 1);
        }
        StopAttacking();
    }

    public void MoveToPosition()
    {
        if (transform.position != fightPosition)
        {
            StartCoroutine(MoveToExactPosition(fightPosition));
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
            newBattle = false;
            battleManager.StartNewBattle();
        }
    }


    public void SetAttackConditions()
    {
        initialPos = transform.position;
        targetReached = false;
        attacking = true;
    }

    public void CheckDeath()
    {
        if(hp <= 0 && inBattle)
        {
            target.GetComponent<Player>().RecieveXP(experiencePoints);
            Destroy(gameObject);
        }
    }



}
