using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : State
{
    protected DataChargeState stateData;
    protected bool isPlayerInMinAggroRange, isDetectingLedge, isDetectingWall, isChargeTimeOver, performCloseRangeAction;
    public ChargeState(Entity e, FiniteStateMachine sm, string s, DataChargeState sd) : base(e, sm, s)
    {
        stateData = sd;
    }


    public override void Enter()
    {
        base.Enter();
        isChargeTimeOver = false;
        entity.SetVelocity(stateData.chargeSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + stateData.chargeTime)
        {
            isChargeTimeOver = true;
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
        isDetectingLedge = entity.CheckLedge();
        isDetectingWall = entity.CheckWall();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }
}
