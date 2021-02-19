using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    
    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage, stunDamageAmount = 1f;

    [SerializeField]
    private Transform attack1HitBoxPos;

    [SerializeField]
    private LayerMask whatIsDamageable;


    private bool gotInput, isAttacking;

    private float lastInputTime = Mathf.NegativeInfinity;

    private Animator anim;

    private AttackDetails attackDetails;

    private Player p;
    private PlayerStats ps;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public float flashTime;

    private void Start()
    {
        anim = transform.GetComponentInChildren<Animator>();
        p = Player.getInstance();
        ps = GetComponent<PlayerStats>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }


    private void Damage(AttackDetails attackDetails)
    {
        FlashInRed();
        int direction;
        ps.ChangeHealth(-attackDetails.damageAmount);
        if (attackDetails.position.x >= transform.position.x)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        p.Knockback(direction);
    }

    private void FlashInRed()
    {
        Color c = new Color(1f, 0.5f, 0.5f, 1f);
        spriteRenderer.color = c;
        Time.timeScale = 0.75f;
        Invoke("ResetColor", flashTime);

    }

    void ResetColor()
    {
        spriteRenderer.color = originalColor;
        Time.timeScale = 1f;
    }


    private void CheckCombatInput()
    {
        if (Time.timeScale != 0)
        {
            if (gameObject.activeSelf)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    gotInput = true;
                    lastInputTime = Time.time;
                }
            }
        }
    }

    private void CheckAttacks()
    {
        if (gotInput)
        {
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                anim.SetBool("isAttacking", isAttacking);
            }
        }
    }

    public void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = 
            Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);

        attackDetails.damageAmount = attack1Damage;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = stunDamageAmount;

        foreach (Collider2D col in detectedObjects)
        {
            col.transform.parent.SendMessage("Damage",attackDetails);
        }
    }

    public void FinishAttack()
    {
        isAttacking = false;
        gotInput = false;
        anim.SetBool("isAttacking", isAttacking);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }
}
