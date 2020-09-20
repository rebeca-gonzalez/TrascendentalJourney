using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected DataMeleeAttack stateData;
    protected AttackDetails attackDetails;
    public MeleeAttackState(Entity e, FiniteStateMachine sm, string s, Transform attackPos, DataMeleeAttack sd) : base(e, sm, s, attackPos)
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
        attackDetails.damageAmount = stateData.damageAmount;
        attackDetails.position = entity.aliveGO.transform.position;
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
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.playerLayer);
        foreach (Collider2D col in detectedObjects)
        {
            col.transform.SendMessage("Damage", attackDetails);
        }
    }
}
