using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Hook this under manager
public class PlayerState : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerData LocalPlayerData;
    public static PlayerState Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        // events

        GameEventManager.OnGameOver += Handle_OnPlayerDead;

        // load saved data 
        LocalPlayerData = PlayerDataSavingHelper.LoadData();
    }

    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnDestroy()
    {
        GameEventManager.OnGameOver -= Handle_OnPlayerDead;
    }

    void Handle_OnPlayerDead(bool isSuccess)
    {
        LocalPlayerData.HowManyTimesDidThePlayerDie++;
        PlayerDataSavingHelper.SaveData();
    }
}

