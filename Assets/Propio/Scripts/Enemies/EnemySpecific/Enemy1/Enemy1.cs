using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Entity
{
    public E1_MoveState moveState { get; private set; }
    public E1_IdleState idleState { get; private set; }
    public E1_PlayerDetectedState playerDetectedState { get; private set; }
    public E1_MeleeAttackState meleeAttackState { get; private set; }
    public E1_LookForPlayerState lookForPlayerState { get; private set; }
    public E1_StunState stunState { get; private set; }
    public E1_DeadState deadState { get; private set; }
    public E1_DodgeState dodgeState { get; private set; }
    public E1_RangedAttackState rangedAttackState { get; private set; }

    public GameObject key;
    public bool hasKey = false;

    [SerializeField]
    private DataMoveState moveStateData;
    [SerializeField]
    private DataIdleState idleStateData;
    [SerializeField]
    private DataPlayerDetected playerDetectedStateData;
    [SerializeField]
    private DataMeleeAttack meleeAttackStateData;
    [SerializeField]
    private DataLookForPlayerState lookForPlayerStateData;
    [SerializeField]
    private DataStunState stunStateData;
    [SerializeField]
    private DataDeadState deadStateData;
    [SerializeField]
    public DataDodgeState dodgeStateData;
    [SerializeField]
    private DataRangedAttackState rangedAttackStateData;


    [SerializeField]
    private Transform meleeAttackPosition;
    [SerializeField]
    private Transform rangedAttackPosition;

    public override void Start()
    {
        base.Start();
        moveState = new E1_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new E1_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new E1_PlayerDetectedState(this,stateMachine, "playerDetected", playerDetectedStateData, this);
        meleeAttackState = new E1_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        lookForPlayerState = new E1_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        stunState = new E1_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new E1_DeadState(this, stateMachine, "dead", deadStateData, this);
        dodgeState = new E1_DodgeState(this, stateMachine, "dodge", dodgeStateData, this);
        rangedAttackState = new E1_RangedAttackState(this, stateMachine, "rangedAttack", rangedAttackPosition, rangedAttackStateData, this);
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
        else if (CheckPlayerInMinAggroRange())
        {
            stateMachine.ChangeState(rangedAttackState);
        }
        else if (!CheckPlayerInMinAggroRange())
        {
            lookForPlayerState.SetFlipInstantly(true);
            stateMachine.ChangeState(lookForPlayerState);
        }
    }
}
