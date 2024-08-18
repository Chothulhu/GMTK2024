using System;
using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    private int currentHealth = 100;
    [SerializeField] private int maxHealth = 100;

    private SpriteRenderer spriteRenderer;
    private float squashThresholdMultiplier = 0.75f; // Determines how small should an enemy be for killing via stomp mechanic
    [SerializeField] private WeaponScript weaponScript;
    private Rigidbody2D rb;

    [SerializeField] private HealthBar healthBar;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
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

            Debug.Log("Enemy: " + enemyHeight);
            Debug.Log("Player: " + playerHeight * squashThresholdMultiplier);
            if (playerHeight * squashThresholdMultiplier >= enemyHeight && rb.velocity.y < 0)
            {
                Debug.Log("hi pookie");
                collision.gameObject.GetComponent<EnemyScript>().TakeDamage(10000);
            } 
        }

        if (collision.tag == "Ammo")
        {    
            ObjectPoolManager.ReturnObjectToPool(collision.gameObject);
            weaponScript.CollectAmmo();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die(); // death scene
        }
    }

    private void Die() {

        Destroy(gameObject);
    
    }

}
