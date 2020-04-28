using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    public int maxAmmo = 20;
    public int ammo = 20;

    public event System.Action ammoChange;


    // Start is called before the first frame update
    void Start()
    {
    }
        // Update is called once per frame
        void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (ammo > 0)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        ammo--;
        ammoChange();
    }
}
