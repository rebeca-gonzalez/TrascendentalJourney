using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E0_LookForPlayerState : LookForPlayerState
{
    private Enemy0 enemy;
    public E0_LookForPlayerState(Entity e, FiniteStateMachine sm, string s, DataLookForPlayerState sd, Enemy0 enem) : base(e, sm, s, sd)
    {
        enemy = enem;
    }

    public override void Checks()
    {
        base.Checks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isPlayerInMinAggroRange)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }
        else if (isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
