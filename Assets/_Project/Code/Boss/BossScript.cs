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
    int rand;

    [SerializeField] private GameObject shockwave;
    [SerializeField] private Transform shockwavePosition;

    [SerializeField] private GameObject acidBall;
    [SerializeField] private Transform[] acidBallPositions;

    [SerializeField] private float scaleSpeed = 0.00000001f; // Speed at which the scale increases
    [SerializeField] private int scaleLast = 10;
    [SerializeField] private float scaleMin = 0.4f;
    private bool isPositiveScaling = false;
    private bool isNegativeScaling = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        abilityCooldown = startAbilityCooldown;
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
        Debug.Log("HIIII");
        startAbilityCooldown = startAbilityCooldown * transform.localScale.x;
    }

    public void CastFirstAbility()
    {
        //ground slam that spawns shockwaves 
        GameObject shock1 = ObjectPoolManager.SpawnObject(shockwave, shockwavePosition.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
        shock1.GetComponent<ShockwaveScript>().direction = false;
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
        for (int i = 0; i < quantity; i++)
        { 
            rand = UnityEngine.Random.Range(0, acidBallPositions.Length);
            ObjectPoolManager.SpawnObject(acidBall, acidBallPositions[rand].position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
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
