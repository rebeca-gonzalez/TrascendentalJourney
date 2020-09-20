using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newIdleStateData", menuName = "Data/State Data/Idle State")]

public class DataIdleState : ScriptableObject
{
    public float minIdleTime = 0.75f, maxIdleTime = 1.5f;
}
