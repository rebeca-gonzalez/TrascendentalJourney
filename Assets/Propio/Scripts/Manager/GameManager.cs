using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Vector3 respawnPoint;
    public int level = 0;
    public bool hasKey = false;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float respawnTime;

    private float respawnTimeStart;
    private bool respawn;
    private CameraFollow cf;

    public event Action respawnEnemies;

    private static GameManager instance;
    private Player p;

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
        p = Player.getInstance();
    }

    private void Update()
    {
        //Debug.Log(1.0f / Time.deltaTime);
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
            Debug.Log("AAAAAAAAA");
            Debug.Log(respawnPoint);
            RespawnEnemies();
            respawn = false;
            //var temp = Instantiate(player, respawnPoint, Quaternion.identity);
            p.gameObject.SetActive(true);
            p.GoToCheckpoint();
            //cf.followObject = temp;
            //cf.rb = cf.followObject.GetComponent<Rigidbody2D>();

        }
    }
}
