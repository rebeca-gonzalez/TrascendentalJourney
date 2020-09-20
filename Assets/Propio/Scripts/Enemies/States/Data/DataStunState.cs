﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newStunStateData", menuName = "Data/State Data/Stun State")]

public class DataStunState : ScriptableObject
{
    public float stunTime = 2f;
    public float stunKnockbackTime = 0.2f;
    public Vector2 stunKnockbackAngle;
    public float stunKnockbackSpeed = 20f;
}
