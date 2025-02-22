using System;
using System.Collections;
using UnityEngine;
using SmallHedge;

public class PlayerScript : MonoBehaviour, DamagableEntity
{

    private int currentHealth = 100;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int dashDamage = 10;

    private SpriteRenderer spriteRenderer;
    private float squashThresholdMultiplier = 0.9f; // Determines how small should an enemy be for killing via stomp mechanic
    [SerializeField] private WeaponScript weaponScript;
    private PlayerMovement playerMovement;
    private GlobalsScript globalsScript;
    private Rigidbody2D rb;
    public GameObject loseScreen;

    [SerializeField] private HealthBar healthBar;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        globalsScript = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GlobalsScript>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        // TEST
        /*if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            float enemyHeight = collision.gameObject.GetComponent<CapsuleCollider2D>().bounds.size.y;
            
            float playerHeight = spriteRenderer.bounds.size.y;

            if (playerHeight * squashThresholdMultiplier >= enemyHeight && rb.velocity.y < 0)
            {
                Debug.Log("rb velocity.y " + rb.velocity.y);
                collision.gameObject.GetComponent<EnemyScript>().TakeDamage(10000);
            }

            if (playerMovement.isDashing)
            {
                if (playerHeight * squashThresholdMultiplier >= enemyHeight) {
                    collision.gameObject.GetComponent<EnemyScript>().TakeDamage(dashDamage + 10000);
                } 
                else
                {
                    collision.gameObject.GetComponent<EnemyScript>().TakeDamage(dashDamage);
                }
                
                SmallHedge.SoundManager.PlaySound(SoundType.DASHYESHIT, null, (float)0.6);
            }
        }

        // vrv moze dosta lepse ali za jam dosta
        if (collision.tag == "Boss")
        {
            float enemyHeight = collision.gameObject.GetComponent<BoxCollider2D>().bounds.size.y;

            float playerHeight = spriteRenderer.bounds.size.y;

            if (playerHeight * squashThresholdMultiplier >= enemyHeight && rb.velocity.y < 0)
            {
                Debug.Log("rb velocity.y " + rb.velocity.y);
                collision.gameObject.GetComponent<EnemyScript>().TakeDamage(10000);
            }

            if (playerMovement.isDashing)
            {
                if (playerHeight * squashThresholdMultiplier >= enemyHeight)
                {
                    collision.gameObject.GetComponent<BossScript>().TakeDamage(dashDamage + 100000);
                }
                else
                {
                    collision.gameObject.GetComponent<BossScript>().TakeDamage(dashDamage);
                }

                SmallHedge.SoundManager.PlaySound(SoundType.DASHYESHIT, null, (float)0.6);
            }
        }

        if (collision.tag == "Ammo")
        {    
            ObjectPoolManager.ReturnObjectToPool(collision.gameObject);
            globalsScript.GetComponent<EnemySpawner>().AddProgress(1);
            weaponScript.CollectAmmo();   
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        SmallHedge.SoundManager.PlaySound(SoundType.DMGTAKEN, null, (float) 0.6);
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die(); // death scene
        }
    }

    private void Die() {
        Destroy(gameObject);
        loseScreen.SetActive(true);
    }

}
