using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyScript : MonoBehaviour, DamagableEntity
{
    private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private float speed;
    private GameObject globals;
    private Transform target;

    [SerializeField] private GameObject itemToDrop;


    [SerializeField] private float scaleSpeed = 0.00000001f; // Speed at which the scale increases
    [SerializeField] private int scaleLast = 10;
    [SerializeField] private float scaleMin = 0.4f;
    private bool isPositiveScaling = false;
    private bool isNegativeScaling = false;

    private void Awake()
    {
        globals = GameObject.FindGameObjectWithTag("GameMaster").gameObject;
        target = globals.GetComponent<GlobalsScript>().playerPosition;
    }

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        //MOVEMENT
        if(target != null)
        {
            float step = speed * Time.deltaTime;
            Vector3 newPos = transform.position;
            newPos.x = Mathf.MoveTowards(newPos.x, target.position.x, step);
            transform.position = newPos;

        }
 


        // SCALING TRIGGERS
        if (isNegativeScaling && (transform.localScale.x > scaleMin && transform.localScale.y > scaleMin && transform.localScale.z > scaleMin))
        {
            // Decrease the scale by X unit per second
            transform.localScale += Vector3.one * -scaleSpeed * Time.deltaTime;
        }

        if (isPositiveScaling)
        {
            // Increase the scale by X unit per second
            transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Hp 0: " + gameObject.name);
            Die();
        }
    }

    public void TriggerScaling(Boolean isPositiveScaling)
    {
        if (isPositiveScaling)
        {
            StartCoroutine(ScaleForSeconds(scaleLast));
        }
        else
        {
            StartCoroutine(ScaleNegativeForSeconds(scaleLast));
        }
    }

    // Method to trigger the scaling for 10 seconds
    private IEnumerator ScaleNegativeForSeconds(int duration)
    {
        isNegativeScaling = true;
        yield return new WaitForSeconds(duration); // Wait for the specified duration
        isNegativeScaling = false;
    }

    private IEnumerator ScaleForSeconds(float duration)
    {
        isPositiveScaling = true;
        yield return new WaitForSeconds(duration); // Wait for the specified duration
        isPositiveScaling = false;
    }

    public void Die()
    {
        Debug.Log("DIED: " + gameObject.name);
        ObjectPoolManager.SpawnObject(itemToDrop, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
        ObjectPoolManager.ReturnObjectToPool(gameObject);

    }


}
