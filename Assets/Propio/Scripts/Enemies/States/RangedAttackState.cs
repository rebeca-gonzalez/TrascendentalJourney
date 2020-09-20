using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : AttackState
{
    protected DataRangedAttackState stateData;
    protected GameObject projectile;
    protected Projectile projectileScript;

    public RangedAttackState(Entity e, FiniteStateMachine sm, string s, Transform attackPos, DataRangedAttackState sd) : base(e, sm, s, attackPos)
    {
        stateData = sd;
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        projectile = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
        projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.FireProjectile(stateData.projectilSpeed, stateData.projectileTravelDistance, stateData.projectileDamage);
    }
}
