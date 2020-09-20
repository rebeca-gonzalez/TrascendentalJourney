using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDispenser : MonoBehaviour
{
    public GameObject projectile;
    private Projectile projectileScript;
    public float timeBetweenArrows, projectilSpeed, projectileTravelDistance, projectileDamage;

    private GameObject proj;
    private float lastArrowShotTime;

    void Start()
    {
        lastArrowShotTime = 0f;
    }

    void Update()
    {
        if (Time.time >= lastArrowShotTime + timeBetweenArrows)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        lastArrowShotTime = Time.time;
        proj = GameObject.Instantiate(projectile, transform.position, transform.rotation);
        projectileScript = proj.GetComponent<Projectile>();
        projectileScript.FireProjectile(projectilSpeed, projectileTravelDistance, projectileDamage);
    }
}
