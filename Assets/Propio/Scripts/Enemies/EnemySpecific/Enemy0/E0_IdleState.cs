using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E0_IdleState : IdleState
{
    private Enemy0 enemy;
    public E0_IdleState(Entity e, FiniteStateMachine sm, string s, DataIdleState sd, Enemy0 enem) : base(e, sm, s, sd)
    {
        enemy = enem;
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

        else if (isIdleTimeOver)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
