using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E0_MeleeAttackState : MeleeAttackState
{
    private Enemy0 enemy;
    public E0_MeleeAttackState(Entity e, FiniteStateMachine sm, string s, Transform attackPos, DataMeleeAttack sd, Enemy0 enem) : base(e, sm, s, attackPos, sd)
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

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
        {
            if (isPlayerInMinAggroRange)
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
            else stateMachine.ChangeState(enemy.lookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}
