using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ShockwaveScript : MonoBehaviour
{
    [HideInInspector] public bool direction;
    [SerializeField] private float speed = 1f;
    [SerializeField] private int damage = 10;
    private void Update()
    {
        if (direction)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<DamagableEntity>();
        if (enemy != null)
        {
            Debug.Log("TONS OF DMG");
            enemy.TakeDamage(damage);
        }
    }

}
