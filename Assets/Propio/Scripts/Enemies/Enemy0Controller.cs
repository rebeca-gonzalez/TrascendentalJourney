using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0Controller : MonoBehaviour
{
    private enum State
    {
        Walking,
        Knockback,
        Dead
    }

    private GameObject alive;
    private Rigidbody2D rb;
    private Animator aliveAnim;

    private State currentState;

    private int facingDirection = 1, damageDirection;
    private bool rayOnGround, rayOnWall;
    private float currentHealth, knockbackStartTime, lastTouchDamageTime;
    private float[] attackDetails = new float[2];

    private Vector2 newVelocity, touchDamageBottomLeft, touchDamageTopRight;

    [SerializeField]
    private Transform groundDetection, wallDetection, touchDamageDetection;

    [SerializeField]
    private LayerMask groundLayer, wallLayer, playerLayer;

    [SerializeField]
    private float groundCheckLength, wallCheckLength, speed, maxHealth, knockbackDuration,
            touchDamageCooldown, touchDamageWidth, touchDamageHeight, touchDamageAmount;

    [SerializeField]
    private Vector2 knockbackSpeed;

    [SerializeField]
    private GameObject hitParticle, chunkParticles;

    private void Start()
    {
        alive = transform.Find("Alive").gameObject;
        rb = alive.GetComponent<Rigidbody2D>();
        aliveAnim = alive.GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Walking:
                UpdateWalkingState();
                break;
            case State.Knockback:
                UpdateKnockbackState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }
    }

    //  WALKING
    private void EnterWalkingState()
    {

    }
    private void UpdateWalkingState()
    {
        rayOnGround = Physics2D.Raycast(groundDetection.position, Vector2.down, groundCheckLength, groundLayer);
        rayOnWall = Physics2D.Raycast(wallDetection.position, transform.right * facingDirection, wallCheckLength, wallLayer) ||
                    Physics2D.Raycast(wallDetection.position, transform.right * facingDirection, wallCheckLength, groundLayer);

        CheckTouchDamage();

        if (!rayOnGround || rayOnWall)
        {
            Flip();
        }
        else
        {
            newVelocity.Set(speed * facingDirection, rb.velocity.y);
            rb.velocity = newVelocity;
        }
    }
    private void ExitWalkingState()
    {

    }

    //  KNOCKBACK
    private void EnterKnockbackState()
    {
        knockbackStartTime = Time.time;
        newVelocity.Set(knockbackSpeed.x * damageDirection, knockbackSpeed.y);
        rb.velocity = newVelocity;
        aliveAnim.SetBool("knockback", true);
    }
    private void UpdateKnockbackState()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration)
        {
            ChangeState(State.Walking);
        }
    }
    private void ExitKnockbackState()
    {
        aliveAnim.SetBool("knockback", false);
    }

    //  DEAD
    private void EnterDeadState()
    {
        //Spawn chunks y sangre
        Instantiate(chunkParticles, alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        Destroy(gameObject);
    }
    private void UpdateDeadState()
    {

    }
    private void ExitDeadState()
    {

    }

    //  MISCELLANIEOUS

    private void Flip()
    {
        facingDirection *= -1;
        alive.transform.Rotate(new Vector3(0, 180, 0));
    }

    private void Damage(float[] attackDetails)
    {
        //attackDetails[0] = float que indica el daño que recibe el enemigo
        //attackDetails[1] = posicion x del jugador para los knockbacks
        currentHealth -= attackDetails[0];
        if (attackDetails[1] > alive.transform.position.x)
        {
            damageDirection = -1;
        }
        else
        {
            damageDirection = 1;
        }

        //particulas
        Instantiate(hitParticle, alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        if (currentHealth > 0.0f)
        {
            ChangeState(State.Knockback);
        }
        else
        {
            ChangeState(State.Dead);
        }
    }

    private void CheckTouchDamage()
    {
        if (Time.time >= lastTouchDamageTime + touchDamageCooldown)
        {
            touchDamageBottomLeft.Set(touchDamageDetection.position.x - (touchDamageWidth / 2), touchDamageDetection.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageDetection.position.x + (touchDamageWidth / 2), touchDamageDetection.position.y + (touchDamageHeight / 2));
            Collider2D hit = Physics2D.OverlapArea(touchDamageBottomLeft, touchDamageTopRight, playerLayer);

            if (hit)
            {
                lastTouchDamageTime = Time.time;
                attackDetails[0] = touchDamageAmount;
                attackDetails[1] = alive.transform.position.x;
                hit.SendMessage("Damage", attackDetails);
;            }
        }
    }

    private void ChangeState(State state)
    {
        switch (currentState)
        {
            case State.Walking:
                ExitWalkingState();
                break;
            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }


        switch (state)
        {
            case State.Walking:
                EnterWalkingState();
                break;
            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }
        currentState = state;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundDetection.position, new Vector2(groundDetection.position.x, groundDetection.position.y - groundCheckLength));
        Gizmos.DrawLine(wallDetection.position, new Vector2(wallDetection.position.x + wallCheckLength, wallDetection.position.y));

        Vector2 botLeft = new Vector2(touchDamageDetection.position.x - (touchDamageWidth / 2), touchDamageDetection.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamageDetection.position.x + (touchDamageWidth / 2), touchDamageDetection.position.y - (touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(touchDamageDetection.position.x - (touchDamageWidth / 2), touchDamageDetection.position.y + (touchDamageHeight / 2));
        Vector2 topRight = new Vector2(touchDamageDetection.position.x + (touchDamageWidth / 2), touchDamageDetection.position.y + (touchDamageHeight / 2));

        Gizmos.DrawLine(botLeft, topLeft);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, botRight);
        Gizmos.DrawLine(botRight, botLeft);
    }
}
