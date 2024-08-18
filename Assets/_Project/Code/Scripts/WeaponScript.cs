using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bullletPrefab;

    // Weapon Rotation
    private Vector3 mousePosition;
    private float angle;
    [SerializeField] private Transform playerTransform;
    

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate and apply rotation from the firePoint to the mouse position
        Vector2 direction = mousePosition - transform.position;
        angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + FixAngle();
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));

        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
    }

    void Shoot() {
        Instantiate(bullletPrefab, firePoint.position, firePoint.rotation);
    }

    //shhhh
    private float FixAngle()
    {
        Vector3 playerMouseDiff = mousePosition - playerTransform.position;
        if (playerMouseDiff.x > 0) return -90f;
        else return 90f;
    }


}
