using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E0_DeadState : DeadState
{
    private Enemy0 enemy;
    public E0_DeadState(Entity e, FiniteStateMachine sm, string s, DataDeadState sd, Enemy0 enem) : base(e, sm, s, sd)
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
