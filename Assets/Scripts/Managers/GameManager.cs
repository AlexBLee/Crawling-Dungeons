using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    public Player playerObject;

    public List<GameObject> cachedEnemyList;

    public Player playerStats;
    public Enemy enemyStats;

    public Enemy[] enemyList;
    public bool[] enemyStates;


    
    // Player/Enemy share the same bool, because its essentially a true false for a player turn.
    public Vector3 lastPlayerPos;
    public Vector3 lastCameraPos;

    public int enemyIndexNumber;
    public int playerLevel;

    public int uiIndexNumber;
    public bool pointerOnSlot;
    public List<Item> itemList;

    public List<AddRemoveStat> addRemoves;

    public BattleManager battleManager;





    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);


    }

    private void Start() 
    {
        enemyStates = new bool[enemyList.Length];
    }

    

    public void DeactivateAdders()
    {
        for(int i = 0; i < addRemoves.Count; i++)
        {
            addRemoves[i].gameObject.SetActive(false);
        }
    }

    public void ActivateAdders()
    {
        for(int i = 0; i < addRemoves.Count; i++)
        {
            addRemoves[i].gameObject.SetActive(true);
        }
    }



}
