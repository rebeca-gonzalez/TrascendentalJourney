using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/State Data/Melee Attack State")]
public class DataMeleeAttack : ScriptableObject
{
    public float attackRadius = 0.5f;
    public LayerMask playerLayer;
    public float damageAmount = 10f;
}
