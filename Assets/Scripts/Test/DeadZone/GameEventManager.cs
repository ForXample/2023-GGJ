using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEventManager : MonoBehaviour
{
    public bool IsGameEnd = false;
    public bool IsGameWin = false;
    public bool IsGameLose = false;
    public bool IsGameOverAnimationFinished = false;

    public delegate void GameOver(bool isFailed);
    public static event GameOver OnGameOver;

    public static GameEventManager Instance { get; private set; }
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
    }

    // Start is called before the first frame update
    void Start()
    {
        //Event Binding
        DeadzoneTrigger.OnDeadzoneTrigger += Handle_OnDeadzoneTriggered;
        WinZoneTrigger.OnWinZoneTrigger += Handle_OnWinzoneTriggered;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsGameEnd)
        {
            OnGameOver(IsGameWin);

            if(IsGameOverAnimationFinished)
            {
                if (IsGameWin)
                {
                    SceneManager.LoadScene("Win Screen");
                }

                if (IsGameLose)
                {
                    SceneManager.LoadScene("Lose Screen");
                }
            }
        }
    }

    void Handle_OnDeadzoneTriggered(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            IsGameEnd = true;
            IsGameLose = true;
        }
        else
        {
            //Could just be platform, just destroy it
            Destroy(collider.gameObject);
        }
    }


    void Handle_OnWinzoneTriggered(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            IsGameEnd = true;
            IsGameWin = true;
        }
    }

    private void OnDisable()
    {
        DeadzoneTrigger.OnDeadzoneTrigger -= Handle_OnDeadzoneTriggered;
    }
}
 