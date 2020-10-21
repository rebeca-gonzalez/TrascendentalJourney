using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingBlock1 : MonoBehaviour
{

    private bool active;
    private float lastSwitchTime;
    private BoxCollider2D bc;

    [SerializeField]
    private float timeBetweenSwitch;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        active = true;
        lastSwitchTime = 0f;
        animator.SetBool("active", active);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= lastSwitchTime + timeBetweenSwitch)
        {
            timeReached();           
        }
    }

    private void timeReached()
    {
        active = !active;
        animator.SetBool("active", active);
        lastSwitchTime = Time.time;
    }

    public void NullifyCollider()
    {
        bc.enabled = false;
    }

    public void ActivateCollider()
    {
        bc.enabled = true;
    }
}
