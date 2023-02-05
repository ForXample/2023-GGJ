using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int HowManyTimesDidThePlayerDie;

    public PlayerData()
    {
        HowManyTimesDidThePlayerDie = 0;
    }
}
