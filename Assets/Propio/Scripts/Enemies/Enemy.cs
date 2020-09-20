using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 20;
    public float speed = 5f;
    public LayerMask groundLayer;

    private bool facingLeft = true;
    private int facingDirection = -1;
    private Rigidbody2D rigidBody;
    private GameObject groundDetection;
    private Vector2 newVelocity;
    private Vector2 direction;
    private bool rayOnGround;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        groundDetection = this.gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        rayOnGround = Physics2D.Raycast(groundDetection.transform.position, Vector2.down, 1f,groundLayer);
    }

    private void FixedUpdate()
    {
        if (!rayOnGround)
        {
            Flip();
        }
        newVelocity.Set(speed * facingDirection, 0);
        rigidBody.velocity = newVelocity;
    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        facingDirection *= -1;
        transform.Rotate(new Vector3(0, 180, 0));
    }
    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
