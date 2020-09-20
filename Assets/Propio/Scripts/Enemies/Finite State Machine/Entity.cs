using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;
    public DataEntity entityData;
    public int facingDirection { get; private set; }
    public int lastDamageDirection { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public GameObject aliveGO { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }
    private Vector2 velocityWorkspace;
    protected Vector2 initialPosition;

    [SerializeField]
    private Transform wallCheck, ledgeCheck, playerCheck, groundCheck;

    private float currentHP, currentStunResistance, lastDamageTime;

    protected bool isStunned, isDead;

    public virtual void Spawn()
    {
        currentHP = entityData.maxHP;
        currentStunResistance = entityData.stunResistance;
        facingDirection = 1;
    }

    public virtual void Start()
    {
        aliveGO = transform.Find("Alive").gameObject;
        rb = aliveGO.GetComponent<Rigidbody2D>();
        anim = aliveGO.GetComponent<Animator>();
        stateMachine = new FiniteStateMachine();
        atsm = aliveGO.GetComponent<AnimationToStateMachine>();
        initialPosition = transform.position;
        Spawn();
        GameManager.getInstance().respawnEnemies += RespawnEnemy;
    }

    public virtual void RespawnEnemy()
    {

    }

    public virtual void Update()
    {
        if (stateMachine==null) Debug.LogError("Entity does not have stateMachine");
        if (stateMachine.currentState == null) Debug.LogError("Entity's stateMachine does not have currentState");
        stateMachine.currentState.LogicUpdate();
        anim.SetFloat("yVelocity", rb.velocity.y);
        if (Time.time >= lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetAngularVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }
    
    public virtual bool CheckPlayerInCloseRangeAction()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.closeRangeActionDistance, entityData.bothLayers);
        return (hit && hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"));
    }

    public virtual bool CheckPlayerInMinAggroRange()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.minAggroDistance, entityData.bothLayers);
        return (hit && hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"));
    }

    public virtual bool CheckPlayerInMaxAggroRange()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.maxAggroDistance, entityData.bothLayers);
        return (hit && hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"));
    }
    
    public virtual bool CheckGround()
    {
        return (Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckRadius, entityData.groundLayer)); 
    }

    public virtual bool CheckWall()
    {
        return (Physics2D.Raycast(wallCheck.position, aliveGO.transform.right, entityData.wallCheckDistance, entityData.groundLayer) ||
                Physics2D.Raycast(wallCheck.position, aliveGO.transform.right, entityData.wallCheckDistance, entityData.wallLayer));
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.groundLayer);
    }

    public virtual void Flip()
    {
        facingDirection *= -1;
        aliveGO.transform.Rotate(new Vector3(0, 180, 0));
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }

    public virtual void Damage (AttackDetails attackDetails)
    {
        Instantiate(entityData.hitParticle, aliveGO.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        currentHP -= attackDetails.damageAmount;
        currentStunResistance -= attackDetails.stunDamageAmount;

        lastDamageTime = Time.time;

        if (attackDetails.position.x > aliveGO.transform.position.x) lastDamageDirection = -1;
        else lastDamageDirection = 1;

        if (currentStunResistance <= 0) isStunned = true;

        if (currentHP > 0) isDead = false;
        else isDead = true;

        DamageHop(entityData.damageHopVelocity);
    }

    public virtual void DamageHop (float velocity)
    {
        velocityWorkspace.Set(rb.velocity.x, velocity);
        rb.velocity = velocityWorkspace;
    }
}
