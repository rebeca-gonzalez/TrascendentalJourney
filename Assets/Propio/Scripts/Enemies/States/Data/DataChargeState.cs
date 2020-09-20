using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newChargeStateData", menuName = "Data/State Data/Charge State")]

public class DataChargeState : ScriptableObject
{
    public float chargeSpeed = 7f;
    public float chargeTime = 1.5f;
}
