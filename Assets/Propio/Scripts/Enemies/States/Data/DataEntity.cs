using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class DataEntity : ScriptableObject
{
    public float wallCheckDistance = 0.25f;
    public float ledgeCheckDistance = 0.3f;
    public float minAggroDistance = 2f;
    public float maxAggroDistance = 4f;
    public float closeRangeActionDistance = 1f;

    public float maxHP = 40f;
    public float damageHopVelocity = 4f;

    public float groundCheckRadius = 0.4f;

    public float stunResistance = 3f;
    public float stunRecoveryTime = 2f;


    public GameObject hitParticle;

    public LayerMask groundLayer, wallLayer, playerLayer, bothLayers;
}
