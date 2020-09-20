using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected DataMoveState stateData;

    protected bool rayOnLedge, rayOnWall;

    protected bool isPlayerInMinAggroRange;

    public MoveState(Entity e, FiniteStateMachine sm, string s, DataMoveState sd) : base(e, sm, s)
    {
        stateData = sd;
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(stateData.movementSpeed);
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

    public override void Checks()
    {
        base.Checks();
        rayOnLedge = entity.CheckLedge();
        rayOnWall = entity.CheckWall();
        isPlayerInMinAggroRange = entity.CheckPlayerInMinAggroRange();
    }

}
