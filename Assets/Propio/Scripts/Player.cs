using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header ("Horizontal Movement")]
    public float speed = 10f;
    public Vector2 direction;
    private bool facingLeft = true;

    [Header("Vertical Movement")]
    public float jumpForce = 15f;
    public float jumpDelay = 0.25f;
    private float jumpTimer;
    private bool hasJumped;
    [SerializeField]
    private float verticalSpeed;

    [Header("Components")]
    private Rigidbody2D rigidBody;
    public Animator animator;
    public LayerMask groundLayer;
    public GameObject characterHolder;

    [Header("Physics")]
    public float maxSpeed = 5f;
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool onGround = false;
    public float groundLength = 1.1f;
    public Vector3 colliderOffset;

    private static Player instance;
    public Weapon weapon;

    //SLOPES
    private EdgeCollider2D cc;
    private Vector2 capsuleColliderSize;

    private float slopeCheckDistance = 1.5f;
    private float maxSlopeAngle = 70f;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;

    public bool isOnSlope;
    public bool canWalkOnSlope;

    private Vector2 slopeNormalPerpendicular;
    private Vector2 newVelocity;


    static public Player getInstance() 
    {
          return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponent<Weapon>();
        rigidBody = GetComponent<Rigidbody2D>();

        cc = GetComponent<EdgeCollider2D>();
        capsuleColliderSize.y = 1.7f;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        bool wasOnGround = onGround;
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) ||
                   Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        if (!Physics2D.Raycast(transform.position, Vector2.down, groundLength + 0.75f, groundLayer))
        {
            //Si aunque no hayas saltado, no detecta suelo, activa la animación falling.
            //Sirve para cuando te tiras de edges.
            animator.SetBool("HasJumped", true);
            hasJumped = true;
        } else
        {
            animator.SetBool("HasJumped", false);
            hasJumped = false;
        }

        if (!wasOnGround && onGround && hasJumped)
        {
            StartCoroutine(JumpSqueeze(1.2f, 0.8f, 0.05f));
            animator.SetBool("HasJumped", false);
            hasJumped = false;
        }
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
        }
    }

    void FixedUpdate()
    {
        SlopeCheck();
        MoveCharacter(direction.x);
        if (jumpTimer > Time.time && onGround)
        {
            Jump();
        }
        ModifyPhysics();
        verticalSpeed = rigidBody.velocity.y;

    }

    private void MoveCharacter(float horizontal)
    {

        if ((onGround && !isOnSlope))
        {
            //rigidBody.AddForce(Vector2.right * horizontal * speed);
            newVelocity.Set(speed * horizontal, 0.0f);
            rigidBody.velocity = newVelocity;
        }
        else if (onGround && isOnSlope && canWalkOnSlope)
        {
            newVelocity.Set(speed * slopeNormalPerpendicular.x * -horizontal, 7 * slopeNormalPerpendicular.y * -horizontal);
            rigidBody.velocity = newVelocity;
        }
        else if (!onGround)
        {
            newVelocity.Set(speed * horizontal, rigidBody.velocity.y);
            rigidBody.velocity = newVelocity;
        }

        animator.SetFloat("Horizontal", Mathf.Abs(rigidBody.velocity.x));
        animator.SetFloat("Vertical", rigidBody.velocity.y);
        

        if ((horizontal > 0 && facingLeft) || (horizontal < 0 && !facingLeft))
        {
            Flip();
        }
        if (Mathf.Abs(rigidBody.velocity.x) > maxSpeed)
        {
            rigidBody.velocity = new Vector2 (Mathf.Sign(rigidBody.velocity.x) * maxSpeed, rigidBody.velocity.y);
        }
    }

    private void Jump()
    {
        animator.SetBool("HasJumped", true);
        hasJumped = true;
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpTimer = 0;
        StartCoroutine(JumpSqueeze(0.8f, 1.2f, 0.1f));
    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, groundLayer);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, groundLayer);

        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }

    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);

        if (hit)
        {

            slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }

            lastSlopeAngle = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerpendicular, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        if (isOnSlope && canWalkOnSlope && direction.x == 0.0f)
        {
            rigidBody.drag = 9999;
        }
        else
        {
            rigidBody.drag = 0;
        }
    }

    private void ModifyPhysics()
    {
        bool changeDirection = (direction.x > 0 && rigidBody.velocity.x < 0) || (direction.x < 0 && rigidBody.velocity.x > 0);
        if (onGround)
        {
            if (Mathf.Abs(direction.x) < 0.4 || changeDirection)
            {
                rigidBody.drag = linearDrag;
            }
            else
            {
                rigidBody.drag = 0f;
            }
            rigidBody.gravityScale = 0f;
        }
        else
        {
            rigidBody.gravityScale = gravity;
            rigidBody.drag = linearDrag * 0.15f;
            if (rigidBody.velocity.y < 0)
            {
                rigidBody.gravityScale = gravity * fallMultiplier;
            } else if (rigidBody.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rigidBody.gravityScale = gravity * (fallMultiplier / 1.5f);
            }
        }
    }

    IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds)
    {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }

    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        transform.Rotate(new Vector3(0, 180, 0));
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position , transform.position + Vector3.down * (groundLength + 0.75f));
    }
}
