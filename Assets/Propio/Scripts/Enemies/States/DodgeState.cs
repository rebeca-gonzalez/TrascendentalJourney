using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : State
{
    protected DataDodgeState stateData;

    protected bool performCloseRangeAction, isPlayerInMaxAggroRange, isGrounded, isDodgeOver;

    public DodgeState(Entity e, FiniteStateMachine sm, string s, DataDodgeState sd) : base(e, sm, s)
    {
        stateData = sd;
    }

    public override void Checks()
    {
        base.Checks();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMaxAggroRange = entity.CheckPlayerInMaxAggroRange();
        isGrounded = entity.CheckGround();
    }

    public override void Enter()
    {
        base.Enter();
        isDodgeOver = false;
        entity.SetAngularVelocity(stateData.dodgeSpeed, stateData.dodgeAngle, -entity.facingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Time.time >= startTime+stateData.dodgeTime && isGrounded)
        {
            isDodgeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
