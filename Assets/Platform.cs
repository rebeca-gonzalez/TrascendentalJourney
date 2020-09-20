using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    public Transform pos1, pos2;
    public float speed;

    private Vector2 nextPos;
    void Start()
    {
        nextPos = pos1.position;
    }
    void Update()
    {
        if(transform.position == pos1.position)
        {
            nextPos = pos2.position;
        }
        else if (transform.position == pos2.position)
        {
            nextPos = pos1.position;
        }
    }
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }
}
