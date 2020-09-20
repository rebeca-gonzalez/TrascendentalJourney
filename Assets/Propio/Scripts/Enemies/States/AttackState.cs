using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    protected Transform attackPosition;
    protected bool isAnimationFinished, isPlayerInMinAggroRange;
    public AttackState(Entity e, FiniteStateMachine sm, string s, Transform attackPos) : base(e, sm, s)
    {
        attackPosition = attackPos;
    }

    public override void Checks()
    {
        base.Checks();
        isPlayerInMinAggroRange = entity.CheckPlayerInMinAggroRange();
    }

    public override void Enter()
    {
        base.Enter();
        isAnimationFinished = false;
        entity.atsm.attackState = this;
        entity.SetVelocity(0f);
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

    public virtual void TriggerAttack()
    {

    }

    public virtual void FinishAttack()
    {
        isAnimationFinished = true;
    }
}
