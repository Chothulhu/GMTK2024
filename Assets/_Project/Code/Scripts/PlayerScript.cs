using System;
using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    private int currentHealth = 100;
    [SerializeField] private int maxHealth = 100;

    private SpriteRenderer spriteRenderer;
    private float squashThresholdMultiplier = 0.75f; // Determines how small should an enemy be for killing via stomp mechanic

    [SerializeField] private HealthBar healthBar;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

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
            if (playerHeight * squashThresholdMultiplier >= enemyHeight)
            {
                collision.gameObject.SetActive(false);
            }
            
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die() {

        Destroy(gameObject);
    
    }

}
