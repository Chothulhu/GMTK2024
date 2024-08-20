using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    private int currentHealth;
    [SerializeField] private int maxHealth = 1000;


    private bool isCasting = false;
    [SerializeField] private float startAbilityCooldown;
    private float abilityCooldown;
    private float baseAbilityCooldown;
    int rand;

    [SerializeField] private GameObject shockwave;
    [SerializeField] private Transform shockwavePosition;

    [SerializeField] private GameObject[] acidBall;
    [SerializeField] private Transform[] acidBallPositions;

    [SerializeField] private float scaleSpeed = 0.00000001f; // Speed at which the scale increases
    [SerializeField] private int scaleLast = 10;
    [SerializeField] private float scaleMin = 0.4f;
    private bool isPositiveScaling = false;
    private bool isNegativeScaling = false;
    [SerializeField] private GameObject bloodParticles;
    [SerializeField] private Transform bloodParticlesPosition;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        abilityCooldown = startAbilityCooldown;
        baseAbilityCooldown = abilityCooldown;
    }

    private void Update()
    {

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

        // ABILITIES

        if (abilityCooldown > 0)
        {
            abilityCooldown -= Time.deltaTime;
        }
        else
        {
            if (!isCasting)
            {
                rand = UnityEngine.Random.Range(0, 2);

                switch (rand)
                {
                    case 0:
                        isCasting = true;
                        CastFirstAbility();
                        break;
                    case 1:
                        isCasting = true;
                        CastSecondAbility();
                        break;
                    case 2:
                        isCasting = true;
                        CastThirdAbility();
                        break;
                    default:
                        break;
                }
            }

            abilityCooldown = startAbilityCooldown;
        }


        // TEST
        if (Input.GetKeyDown(KeyCode.E))
        {
            CastFirstAbility();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CastSecondAbility();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            CastThirdAbility();
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

    public void ScaleAbilityCooldown()
    {
        startAbilityCooldown = baseAbilityCooldown * transform.localScale.x;
    }

    public void CastFirstAbility()
    {
        //ground slam that spawns shockwaves 
        GameObject shock1 = ObjectPoolManager.SpawnObject(shockwave, shockwavePosition.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
        shock1.GetComponent<ShockwaveScript>().direction = false;
        Vector2 localScale1 = shock1.GetComponent<Transform>().localScale;
        localScale1.x *= -1f;
        shock1.GetComponent<Transform>().localScale = localScale1;
        GameObject shock2 = ObjectPoolManager.SpawnObject(shockwave, shockwavePosition.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
        shock2.GetComponent<ShockwaveScript>().direction = true;
        
        isCasting = false;
    }

    public void CastSecondAbility()
    {
        //magic that spawns aerial projectiles 
        StartCoroutine(SpawnAcidRain(0.2f, 10));
    }

    public void CastThirdAbility()
    {
        // collect all uncollected globs and start healing for each one
        StartCoroutine(CollectOrbs(0.2f));
    }

    private IEnumerator SpawnAcidRain(float timeBetweenProjectiles, int quantity)
    {
        int rand;
        int rand2;
        for (int i = 0; i < quantity; i++)
        { 
            rand = UnityEngine.Random.Range(0, acidBallPositions.Length);
            rand2 = UnityEngine.Random.Range(0, acidBall.Length);
            ObjectPoolManager.SpawnObject(acidBall[rand2], acidBallPositions[rand].position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
            yield return new WaitForSeconds(timeBetweenProjectiles);
        }

        isCasting = false;
    }

    private IEnumerator CollectOrbs(float delay)
    {
        // collect all uncollected globs and start healing for each one
        GameObject[] drops = GameObject.FindGameObjectsWithTag("Ammo");
        yield return new WaitForSeconds(delay);
        foreach (GameObject drop in drops)
        {
            DropScript dropScript = drop.GetComponent<DropScript>();
            if (dropScript != null)
            {
                dropScript.GoToBoss();
            }

        }
        isCasting = false;
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

        ObjectPoolManager.SpawnObject(bloodParticles, bloodParticlesPosition.position, Quaternion.identity, ObjectPoolManager.PoolType.ParticleSystem);
        isNegativeScaling = true;
        yield return new WaitForSeconds(duration); // Wait for the specified duration
        isNegativeScaling = false;
        ScaleAbilityCooldown();
    }

    private IEnumerator ScaleForSeconds(float duration)
    {
        isPositiveScaling = true;
        yield return new WaitForSeconds(duration); // Wait for the specified duration
        isPositiveScaling = false;
        ScaleAbilityCooldown();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ammo")
        {
            ObjectPoolManager.ReturnObjectToPool(collision.gameObject);
            TriggerScaling(true);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die(); // death scene
        }
    }

    private void Die()
    {
        Debug.Log("GameOver");
    }
}
