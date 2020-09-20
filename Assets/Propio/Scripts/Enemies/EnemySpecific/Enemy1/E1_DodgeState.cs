using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_DodgeState : DodgeState
{
    private Enemy1 enemy;
    public E1_DodgeState(Entity e, FiniteStateMachine sm, string s, DataDodgeState sd, Enemy1 enem) : base(e, sm, s, sd)
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
        if (isDodgeOver)
        {
            if(isPlayerInMaxAggroRange && performCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.meleeAttackState);
            }
            else if (isPlayerInMaxAggroRange && !performCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.rangedAttackState);
            }
            else if (!isPlayerInMaxAggroRange)
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
