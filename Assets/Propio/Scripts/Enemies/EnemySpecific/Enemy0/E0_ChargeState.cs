using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E0_ChargeState : ChargeState
{
    private Enemy0 enemy;
    public E0_ChargeState(Entity e, FiniteStateMachine sm, string s, DataChargeState sd, Enemy0 enem) : base(e, sm, s, sd)
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

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
        else if (!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
        else if (isChargeTimeOver)
        {
             if (isPlayerInMinAggroRange)
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
             else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
