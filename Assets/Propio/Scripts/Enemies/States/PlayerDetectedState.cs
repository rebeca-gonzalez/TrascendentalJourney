using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State
{
    protected DataPlayerDetected stateData;
    protected bool isPlayerInMinAggroRange, isPlayerInMaxAggroRange, performLongRangeAction, performCloseRangeAction;

    public PlayerDetectedState(Entity e, FiniteStateMachine sm, string s, DataPlayerDetected sd) : base(e, sm, s)
    {
        stateData = sd;
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(0f);
        performLongRangeAction = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + stateData.longRangeActionTime)
        {
            performLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

    public override void Checks()
    {
        base.Checks();

        isPlayerInMinAggroRange = entity.CheckPlayerInMinAggroRange();
        isPlayerInMaxAggroRange = entity.CheckPlayerInMaxAggroRange();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }

}
