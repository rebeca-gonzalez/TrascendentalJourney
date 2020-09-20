using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private Animator anim;

    private bool playerDetected;
    private Player p;

    private void Start()
    {
        p = Player.getInstance();
    }

    void Update()
    {
        playerDetected = Physics2D.OverlapCircle(transform.position, 1.5f,playerLayer);
        if (playerDetected)
        {
            if (p.hasKey)
            {
                anim.SetTrigger("destroy");
            }
        }
    }

    public void DestroyDoor()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}
