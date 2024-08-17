using System;
using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] private int health = 100;

    private SpriteRenderer spriteRenderer;
    private float squashThresholdMultiplier = 0.75f; // Determines how small should an enemy be for killing via stomp mechanic

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        health -= damage;
        if (health < 0)
        {
            Die();
        }
    }

    private void Die() {

        Destroy(gameObject);
    
    }

}
