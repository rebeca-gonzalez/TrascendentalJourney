using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E0_PlayerDetectedState : PlayerDetectedState
{
    private Enemy0 enemy;
    public E0_PlayerDetectedState(Entity e, FiniteStateMachine sm, string s, DataPlayerDetected sd, Enemy0 enem) : base(e, sm, s, sd)
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
        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
        else if (performLongRangeAction)
        {
            stateMachine.ChangeState(enemy.chargeState);
        }
        else if (!isPlayerInMaxAggroRange)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
