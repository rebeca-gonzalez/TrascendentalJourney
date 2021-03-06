﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float health;
    public float[] pos;
    public int level;
    public bool hasKey;

    public PlayerData (Player p)
    {
        health = p.ps.currentHealth;
        level = p.level;
        hasKey = p.hasKey;
        pos = new float[3];
        pos[0] = p.transform.position.x;
        pos[1] = p.transform.position.y;
        pos[2] = p.transform.position.z;
    }
}
