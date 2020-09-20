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
    private int facingDirection = -1;
    public float movementForceInAir;

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
    public LayerMask wallLayer;

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


    private bool canFlip = true;

    //SLOPES
    private CapsuleCollider2D cc;
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


    //WALL RELATED
    public bool isTouchingWall;
    public float wallCheckDistance;
    public bool isWallSliding;
    public float wallSlideSpeed;
    public Vector2 wallJumpDirection;
    public float wallJumpForce;
    private Vector3 wallCheckOffset = new Vector3(0, 0.3f, 0);

    //KNOCKBACK
    private bool knockback;
    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;
    [SerializeField]
    private Vector2 knockbackSpeed;


    public PlayerCombatController pcc;
    public PlayerStats ps;
    private GameManager GM;

    private Dialog dialog;

    public bool hasKey;

    public Transform groundCheck;
    public float groundCheckRadius;

    static public Player getInstance() 
    {
          return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        Flip();
        weapon = GetComponent<Weapon>();
        rigidBody = GetComponent<Rigidbody2D>();
        pcc = GetComponent <PlayerCombatController>();
        ps = GetComponent<PlayerStats>();
        cc = GetComponent<CapsuleCollider2D>();
        capsuleColliderSize.y = 1.7f;
        wallJumpDirection.Normalize();
        GM = GameManager.getInstance();
        dialog = GameObject.Find("Dialog Manager").GetComponent<Dialog>();
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
        onGround = //Physics2D.OverlapCapsule(groundCheck.position, new Vector2(cc.size.x - 0.1f, cc.size.y), CapsuleDirection2D.Vertical, 0f, groundLayer);
        Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        /*Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) ||
        Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer) ||
        Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, wallLayer) ||
        Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, wallLayer);*/
        if (Input.GetKeyDown("k")) SavePlayer();
        if (Input.GetKeyDown("l")) LoadPlayer();


        if //(!onGround)
            (!Physics2D.Raycast(transform.position, Vector2.down, groundLength + 0.75f, groundLayer) &&
            !Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength + 0.75f, groundLayer) &&
            !Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength + 0.75f, groundLayer))
        {
            //Si aunque no hayas saltado, no detecta suelo, activa la animación falling.
            //Sirve para cuando te tiras de edges.
            
            animator.SetBool("HasJumped", true);
            hasJumped = true;
        } 

        if (!wasOnGround && onGround && hasJumped)
        {
            StartCoroutine(JumpSqueeze(1.2f, 0.8f, 0.05f));
            animator.SetBool("HasJumped", false);
            hasJumped = false;
            animator.SetTrigger("Landed");
            //transform.position = new Vector3(transform.position.x, Mathf.Ceil(transform.position.y));
        }

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
            CheckForWallJump();
        }
        CheckForWalls();
        CheckIfWallSliding();
        CheckKnockback();
    }

    private void CheckForWalls()
    {
        isTouchingWall = Physics2D.Raycast(transform.position - wallCheckOffset, -transform.right, wallCheckDistance, wallLayer) ||
                         Physics2D.Raycast(transform.position - wallCheckOffset, -transform.right, wallCheckDistance, groundLayer);
    }


    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !onGround && rigidBody.velocity.y <= 0) 
        {
            isWallSliding = true;
            animator.SetBool("isWallSliding", true);
        }
        else
        {
            isWallSliding = false;
            animator.SetBool("isWallSliding", false);
        }
    }

    private void CheckForWallJump()
    {
        if ((isWallSliding || isTouchingWall) && !onGround)
        {
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * -facingDirection, wallJumpForce * wallJumpDirection.y);
            rigidBody.AddForce(forceToAdd, ForceMode2D.Impulse);
            Flip();
        }
    }

    void FixedUpdate()
    {
            //SlopeCheck();
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
        if (!isWallSliding)
        {
            if ((onGround && !isOnSlope) && !knockback)
            {
                //rigidBody.AddForce(Vector2.right * horizontal * speed);
                newVelocity.Set(speed * horizontal, 0.0f);
                rigidBody.velocity = newVelocity;
            }
            /*else if (onGround && isOnSlope && canWalkOnSlope && !knockback)
            {
                newVelocity.Set(speed * slopeNormalPerpendicular.x * -horizontal, 7 * slopeNormalPerpendicular.y * -horizontal);
                rigidBody.velocity = newVelocity;
            }*/
            else if (!onGround && !knockback)
            {
                Vector2 forceToAdd = new Vector2(movementForceInAir * horizontal, 0);
                rigidBody.AddForce(forceToAdd);
            }

            if ((horizontal > 0 && facingLeft) || (horizontal < 0 && !facingLeft))
            {
                Flip();
            }
        }
        animator.SetFloat("Horizontal", Mathf.Abs(rigidBody.velocity.x));
        animator.SetFloat("Vertical", rigidBody.velocity.y);
        

        
        if (Mathf.Abs(rigidBody.velocity.x) > maxSpeed)
        {
            rigidBody.velocity = new Vector2 (Mathf.Sign(rigidBody.velocity.x) * maxSpeed, rigidBody.velocity.y);
        }
    }

    private void Jump()
    {
        animator.SetBool("HasJumped", true);
        //transform.position = new Vector3(transform.position.x, Mathf.Ceil(transform.position.y)+2);
        hasJumped = true;
        //rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        //rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
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
        if (onGround && verticalSpeed == 0)
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
            }
            else if (rigidBody.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rigidBody.gravityScale = gravity * (fallMultiplier / 1.5f);
            }
        }
        if (isWallSliding)
        {
            if (rigidBody.velocity.y < -wallSlideSpeed)
            {
                rigidBody.velocity = new Vector2(0f, -wallSlideSpeed);
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
            transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }

    }

    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

    private void Flip()
    {
        if (canFlip && !knockback)
        {
            facingLeft = !facingLeft;
            facingDirection *= -1;
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rigidBody.velocity = new Vector2(0.0f, rigidBody.velocity.y);
        }
    }

    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rigidBody.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        AttackDetails a = new AttackDetails();
        a.damageAmount = 999f;

        if(collision.tag == "Spikes")
        {
            pcc.SendMessage("Damage", a);
        }
        else if (collision.tag == "Checkpoint")
        {
            GM.respawnPoint = transform.position;
        }
        else if (collision.tag == "DialogTrigger")
        {
            dialog.NextSentence();
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "Key")
        {
            hasKey = true;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            Debug.Log("Puerrta");
        }
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        ps.currentHealth = data.health;
        Vector3 pos;
        pos.x = data.pos[0];
        pos.y = data.pos[1];
        pos.z = data.pos[2];

        transform.position = pos;

        ps.ChangeHealth(0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
