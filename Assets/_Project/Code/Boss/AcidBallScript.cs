using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBallScript : MonoBehaviour
{
    private Transform playerPosition;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        transform.Rotate(0f, 0f, Random.Range(0f, 360f));
        
    }

    private void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GlobalsScript>().playerPosition;

        Vector2 direction = playerPosition.position - transform.position;
        rb.velocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<DamagableEntity>();
        if (enemy != null)
        {
            Debug.Log("TONS OF DMG");
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
