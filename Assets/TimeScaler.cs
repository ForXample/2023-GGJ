using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    // Start is called before the first frame update

    public float timefadescaler;
    private float fixedDeltaTime;
    void Awake()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;
        GameEventManager.OnGameOver += Handle_OnGameOver;
    }

    void Handle_OnGameOver(bool isGameWin)
    {
        StartCoroutine(FadeTimeScale(isGameWin));
    }

    private void OnDestroy()
    {
        GameEventManager.OnGameOver -= Handle_OnGameOver;
    }

    IEnumerator FadeTimeScale(bool isGameWin)
    {
        if(Time.timeScale > 0)
        {
            Time.timeScale -= Time.deltaTime * timefadescaler;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            GameEventManager.Instance.IsGameOverAnimationFinished = false;
        }
        
        if(Time.timeScale <= 0.2)
        {
            Time.timeScale = 0;
            GameEventManager.Instance.IsGameOverAnimationFinished = true;

        }

        yield return null;
    }
}
