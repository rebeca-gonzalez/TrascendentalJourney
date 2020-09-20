using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRangedAttackStateData", menuName = "Data/State Data/Ranged Attack State")]

public class DataRangedAttackState : ScriptableObject
{
    public GameObject projectile;
    public float projectileDamage = 5f;
    public float projectilSpeed = 15f;
    public float projectileTravelDistance;
}
