﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private AttackDetails attackDetails;
    private float speed, travelDistance, xStartPosition, timeHitGround;
    private Rigidbody2D rb;
    private bool isGravityOn, hasHitGround;

    [SerializeField]
    private float gravity;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask wallLayer;
    [SerializeField]
    private Transform damagePosition;
    [SerializeField]
    private float damageRadius;
    [SerializeField]
    private float timeToDespawn;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGravityOn = false;
        rb.gravityScale = 0f;
        rb.velocity = transform.right * speed;
        xStartPosition = transform.position.x;
        timeHitGround = Mathf.Infinity;
    }
    private void Update()
    {
        if (!hasHitGround)
        {
            attackDetails.position = transform.position;
            if (isGravityOn)
            {
                float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
        if (Time.time >= timeHitGround + timeToDespawn) Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        if (!hasHitGround)
        {
            Collider2D damageHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, playerLayer);
            Collider2D groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, groundLayer);
            Collider2D wallHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, wallLayer);

            if (damageHit)
            {
                damageHit.transform.SendMessage("Damage", attackDetails);
                Destroy(gameObject);
            }

            if (groundHit || wallHit)
            {
                hasHitGround = true;
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
                timeHitGround = Time.time;
            }

            if (Mathf.Abs(xStartPosition - transform.position.x) >= travelDistance && !isGravityOn)
            {
                isGravityOn = true;
                rb.gravityScale = gravity;
            }
        }
    }
    public void FireProjectile(float speed, float travelDistance, float damage)
    {
        this.speed = speed;
        this.travelDistance = travelDistance;
        attackDetails.damageAmount = damage;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }
}
