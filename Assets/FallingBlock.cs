using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{

    private bool playerDetected, hasAlreadyDetectedPlayer;
    private float playerDetectedTime;
    private BoxCollider2D bc;

    [SerializeField]
    private float rayLength;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private Vector3 rayOffset;
    [SerializeField]
    private float blockRespawnTime;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        hasAlreadyDetectedPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        playerDetected = Physics2D.Raycast(transform.position, Vector2.up, rayLength, playerLayer) ||
                         Physics2D.Raycast(transform.position + rayOffset, Vector2.up, rayLength, playerLayer) ||
                         Physics2D.Raycast(transform.position - rayOffset, Vector2.up, rayLength, playerLayer);

        animator.SetBool("playerDetected", playerDetected);

        if (playerDetected && !hasAlreadyDetectedPlayer)
        {
            playerDetectedTime = Time.time;
            hasAlreadyDetectedPlayer = true;
        }

        if( Time.time >= playerDetectedTime + blockRespawnTime)
        {
            animator.SetTrigger("respawn");
            bc.enabled = true;
            hasAlreadyDetectedPlayer = false;
        }
    }

    public void NullifyCollider()
    {
        bc.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + rayOffset, transform.position + rayOffset + Vector3.up * rayLength);
        Gizmos.DrawLine(transform.position - rayOffset, transform.position - rayOffset + Vector3.up * rayLength);
    }

}
