using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected FiniteStateMachine stateMachine;
    protected Entity entity;
    public float startTime { get; protected set; }
    protected string animBoolName;

    public State(Entity e, FiniteStateMachine sm, string s)
    {
        entity = e;
        stateMachine = sm;
        animBoolName = s;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
        entity.anim.SetBool(animBoolName, true);
        Checks();
    }

    public virtual void Exit()
    {
        entity.anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        Checks();
    }

    public virtual void Checks()
    {

    }
}
