using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    protected DataDeadState stateData;
    public DeadState(Entity e, FiniteStateMachine sm, string s, DataDeadState sd) : base(e, sm, s)
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

        GameObject.Instantiate(stateData.deathBloodParticles, entity.aliveGO.transform.position, stateData.deathBloodParticles.transform.rotation);
        GameObject.Instantiate(stateData.deathChunkParticles, entity.aliveGO.transform.position, stateData.deathChunkParticles.transform.rotation);

        entity.gameObject.SetActive(false);
    }

    public override void Exit()
    {
        base.Exit();
        entity.gameObject.SetActive(true);
        entity.Spawn();
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
