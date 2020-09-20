using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0 : Entity
{
    public E0_IdleState idleState { get; private set; }
    public E0_MoveState moveState { get; private set; }
    public E0_PlayerDetectedState playerDetectedState { get; private set; }
    public E0_ChargeState chargeState { get; private set; }
    public E0_LookForPlayerState lookForPlayerState { get; private set; }
    public E0_MeleeAttackState meleeAttackState { get; private set; }
    public E0_StunState stunState { get; private set; }
    public E0_DeadState deadState { get; private set; }

    public GameObject key;
    public bool hasKey = false;

    [SerializeField]
    private DataIdleState idleStateData;
    [SerializeField]
    private DataMoveState moveStateData;
    [SerializeField]
    private DataPlayerDetected playerDetectedData;
    [SerializeField]
    private DataChargeState chargeStateData;
    [SerializeField]
    private DataLookForPlayerState lookForPlayerStateData;
    [SerializeField]
    private Transform meleeAttackPosition;
    [SerializeField]
    private DataMeleeAttack meleeAttackStateData;
    [SerializeField]
    private DataStunState stunStateData;
    [SerializeField]
    private DataDeadState deadStateData;


    public override void Start()
    {
        base.Start();
        moveState = new E0_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new E0_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new E0_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        chargeState = new E0_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new E0_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new E0_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new E0_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new E0_DeadState(this, stateMachine, "dead", deadStateData, this);
        stateMachine.Initialize(moveState);
    }

    public override void RespawnEnemy()
    {
        if (!gameObject.activeSelf)
        {
            Debug.Log(initialPosition);
            stateMachine.ChangeState(idleState);
            aliveGO.transform.SetPositionAndRotation(initialPosition, Quaternion.identity);
        }
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if (isDead)
        {
            if (hasKey)
            {
                Instantiate(key, aliveGO.transform.position, Quaternion.identity);
            }
            stateMachine.ChangeState(deadState);
        }
        else if (isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
        else if (!CheckPlayerInMinAggroRange())
        {
            lookForPlayerState.SetFlipInstantly(true);
            stateMachine.ChangeState(lookForPlayerState);
        }
    }
}
