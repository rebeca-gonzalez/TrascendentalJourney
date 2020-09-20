using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Vector3 respawnPoint;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float respawnTime;

    private float respawnTimeStart;
    private bool respawn;
    private CameraFollow cf;

    public event Action respawnEnemies;

    private static GameManager instance;

    public void RespawnEnemies()
    {
        if(respawnEnemies != null)
        {
            respawnEnemies();
        }
    }

    static public GameManager getInstance()
    {
        return (instance);
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        cf = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
    }

    private void Update()
    {
        CheckRespawn();
        if (Input.GetKeyDown("r")) RespawnEnemies();

    }

    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }

    private void CheckRespawn()
    {
        if (Time.time >= respawnTimeStart + respawnTime && respawn)
        {
            RespawnEnemies();
            respawn = false;
            var temp = Instantiate(player, respawnPoint, Quaternion.identity);
            cf.followObject = temp;
            cf.rb = cf.followObject.GetComponent<Rigidbody2D>();

        }
    }
}
