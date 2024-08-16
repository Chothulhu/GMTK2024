using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bullletPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            shoot();
        }
    }

    void shoot() {
        Instantiate(bullletPrefab, firePoint.position, firePoint.rotation);

    }
}
