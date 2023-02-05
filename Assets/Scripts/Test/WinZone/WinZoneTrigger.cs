using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZoneTrigger : MonoBehaviour
{
    public delegate void WinZoneTriggerDelegate(Collider2D collider);
    public static event WinZoneTriggerDelegate OnWinZoneTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnWinZoneTrigger(collision);
    }
}
