using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E0_StunState : StunState
{
    private Enemy0 enemy;
    public E0_StunState(Entity e, FiniteStateMachine sm, string s, DataStunState sd, Enemy0 enem) : base(e, sm, s, sd)
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
        if (isStunTimeOver)
        {
            if (performCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.meleeAttackState);
            }
            else if (isPlayerInMinAggroRange)
            {
                stateMachine.ChangeState(enemy.chargeState);
            }
            else
            {
                enemy.lookForPlayerState.SetFlipInstantly(true);
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
