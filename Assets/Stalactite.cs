using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour
{
    private Player p;
    private bool playerDetected, hasAlreadyDetectedPlayer, firstDetected;
    private float playerDetectedTime;
    private BoxCollider2D bc;
    private AttackDetails a;
    private Vector3 initialPosition, pos;


    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float blockRespawnTime;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rayLength;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float damageRadius;


    void Start()
    {
        p = Player.getInstance();
        bc = GetComponent<BoxCollider2D>();
        hasAlreadyDetectedPlayer = false;
        a.damageAmount = 999f;
        initialPosition = transform.position;
        pos = transform.position + Vector3.down * rayLength;
        firstDetected = true;
    }

    void Update()
    {
        playerDetected = Physics2D.Raycast(transform.position, Vector2.down, rayLength, playerLayer);

        if (playerDetected || hasAlreadyDetectedPlayer)
        {
            hasAlreadyDetectedPlayer = true;
            transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);

            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position - offset, damageRadius, playerLayer);
            foreach (Collider2D col in detectedObjects)
            {
                col.transform.SendMessage("Damage", a);
            }
            if (firstDetected)
            {
                playerDetectedTime = Time.time;
                firstDetected = false;
            }
        }

        if (Time.time >= playerDetectedTime + blockRespawnTime)
        {
            hasAlreadyDetectedPlayer = false;
            firstDetected = true;
            Respawn();
        }
    }


    private void Respawn()
    {
        transform.position = Vector3.Lerp(transform.position, initialPosition, 1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position , transform.position +  Vector3.down * rayLength);
    }
}
