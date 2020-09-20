using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected DataIdleState stateData;
    protected bool flipAfterIdle, isIdleTimeOver;
    protected float idleTime;

    protected bool isPlayerInMinAggroRange;

    public IdleState(Entity e, FiniteStateMachine sm, string s, DataIdleState sd) : base(e, sm, s)
    {
        stateData = sd;
    }


    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(0f);
        isIdleTimeOver = false;
        RandomizeIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if (flipAfterIdle)
        {
            entity.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
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
    }

    public void SetFlipAfterIdle(bool b)
    {
        flipAfterIdle = b;
    }

    private void RandomizeIdleTime()
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }
}
