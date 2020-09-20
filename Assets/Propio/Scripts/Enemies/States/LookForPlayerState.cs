using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayerState : State
{
    protected DataLookForPlayerState stateData;

    protected bool isPlayerInMinAggroRange, areAllTurnsDone, isAllTurnsTimeDone, flipInstantly;

    protected float lastTurnTime;

    protected int amountOfTurnsDone;

    public LookForPlayerState(Entity e, FiniteStateMachine sm, string s, DataLookForPlayerState sd) : base(e, sm, s)
    {
        stateData = sd;
    }

    public override void Checks()
    {
        base.Checks();
        isPlayerInMinAggroRange = entity.CheckPlayerInMinAggroRange();
    }

    public override void Enter()
    {
        base.Enter();
        isPlayerInMinAggroRange = false;
        areAllTurnsDone = false;
        isAllTurnsTimeDone = false;
        //flipInstantly = false;

        lastTurnTime = startTime;
        amountOfTurnsDone = 0;

        entity.SetVelocity(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (flipInstantly)
        {
            entity.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
            flipInstantly = false;
        }else if (Time.time >= lastTurnTime + stateData.timeInTurns && !areAllTurnsDone){
            entity.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
        }

        if (amountOfTurnsDone == stateData.amountOfTurns)
        {
            areAllTurnsDone = true;
        }
        if ( Time.time >= lastTurnTime + stateData.timeInTurns && areAllTurnsDone)
        {
            isAllTurnsTimeDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetFlipInstantly(bool b)
    {
        flipInstantly = b;
    }
}
