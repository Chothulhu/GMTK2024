using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using SmallHedge;
using UnityEditor.Tilemaps;

public class EnemyScript : MonoBehaviour, DamagableEntity
{
    private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] GameObject bloodParticlesExplosion;
    public float speed;

    private GameObject globals;
    private Transform target;
    
    private bool isFacingRight;
    [SerializeField] private bool isFacingRightAtStart;

    [SerializeField] private GameObject itemToDrop;

    private bool isPositiveScaling = false;
    private bool isNegativeScaling = false;
    [SerializeField] private float scaleSpeed = 0.00000001f; // Speed at which the scale increases
    [SerializeField] private int scaleLast = 10;
    [SerializeField] private float scaleMin = 0.4f;
    [SerializeField] private float scaleMax = 2f;
    [SerializeField] private GameObject bloodParticles;
    [SerializeField] private Transform bloodParticlesPosition;
    [SerializeField] private GameObject[] bloodSplatter;

    private void Awake()
    {
        globals = GameObject.FindGameObjectWithTag("GameMaster").gameObject;
        target = globals.GetComponent<GlobalsScript>().playerPosition;
    }

    private void Start()
    {
        health = maxHealth;
        isFacingRight = isFacingRightAtStart;
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

        if (isPositiveScaling && (transform.localScale.x < scaleMax && transform.localScale.y < scaleMax && transform.localScale.z < scaleMax))
        {
            // Increase the scale by X unit per second
            transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;
        }

        FlipSprite();
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Hp 0: " + gameObject.name);
            Die();
            SmallHedge.SoundManager.PlaySound(SoundType.NPCDEATH, null, 0.3f);
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
        //fml (zaustavimo particleSystem da bi postavili duzinu trajanja kao i za scalingDown)
        var particle = ObjectPoolManager.SpawnObject(bloodParticles, bloodParticlesPosition.position, Quaternion.identity, ObjectPoolManager.PoolType.ParticleSystem);
        particle.transform.SetParent(gameObject.transform);
        var particleSystem = particle.GetComponent<ParticleSystem>();
        particleSystem.Stop();
        var main = particleSystem.main;
        main.duration = duration;
        particleSystem.Play();

        isNegativeScaling = true;
        yield return new WaitForSeconds(duration); // Wait for the specified duration
        isNegativeScaling = false;
    }

    private IEnumerator ScaleForSeconds(int duration)
    {
        isPositiveScaling = true;
        yield return new WaitForSeconds(duration); // Wait for the specified duration
        isPositiveScaling = false;
    }

    private void FlipSprite()
    {
        var diff = target.position - transform.position;
        if (isFacingRight && diff.x < 0f || !isFacingRight && diff.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }    
        
    }

 /*   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ammo")
        {
            ObjectPoolManager.ReturnObjectToPool(collision.gameObject);
            TriggerScaling(true);
        }
    }*/

    public void Die()
    {
        Debug.Log("DIED: " + gameObject.name);
        var id =  UnityEngine.Random.Range(0, bloodSplatter.Length);
        Instantiate(bloodParticlesExplosion, transform.position - new Vector3(0, 0.8f, 0), Quaternion.identity);
        Instantiate(bloodSplatter[id], transform.position - new Vector3(0, 0.8f, 0), Quaternion.identity);
        ObjectPoolManager.SpawnObject(itemToDrop, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }


}
