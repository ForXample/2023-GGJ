using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadzoneTrigger : MonoBehaviour
{
    public delegate void DeadzoneTriggerDelegate(Collider2D collider);
    public static event DeadzoneTriggerDelegate OnDeadzoneTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HIT");
        OnDeadzoneTrigger(collision);
    }

}
