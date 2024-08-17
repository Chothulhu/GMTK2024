using System;
using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] private int scaleSpeed = 1; // Speed at which the scale increases
    [SerializeField] private int scaleLast = 10;
    [SerializeField] private int health = 100;
    private bool isPositiveScaling = false;
    private bool isNegativeScaling = false;

    private SpriteRenderer spriteRenderer;
    private float squashThresholdMultiplier = 0.75f; // Determines how small should an enemy be for killing via stomp mechanic

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isNegativeScaling)
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


    // Method to trigger the scaling for 10 seconds
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

    public void takeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            die();
        }
    }

    void die() {

        Destroy(gameObject);
    
    }

}
