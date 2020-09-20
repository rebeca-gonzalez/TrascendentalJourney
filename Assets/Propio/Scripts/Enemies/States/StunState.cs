using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    protected DataStunState stateData;
    protected bool isStunTimeOver, isGrounded, isMovementStopped, performCloseRangeAction, isPlayerInMinAggroRange;

    public StunState(Entity e, FiniteStateMachine sm, string s, DataStunState sd) : base(e, sm, s)
    {
        stateData = sd;
    }

    public override void Checks()
    {
        base.Checks();
        isGrounded = entity.CheckGround();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMinAggroRange = entity.CheckPlayerInMinAggroRange();
    }

    public override void Enter()
    {
        base.Enter();
        isStunTimeOver = false;
        isMovementStopped = false;
        entity.SetAngularVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle, entity.lastDamageDirection);
    }

    public override void Exit()
    {
        base.Exit();
        entity.ResetStunResistance();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + stateData.stunTime)
        {
            isStunTimeOver = true;
        }
        if (isGrounded && Time.time >= startTime + stateData.stunKnockbackTime && !isMovementStopped)
        {
            entity.SetVelocity(0f);
            isMovementStopped = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
