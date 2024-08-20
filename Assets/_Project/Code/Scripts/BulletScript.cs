using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public int damage = 40;
    // Start is called before the first frame update
    void OnEnable()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the bullet to the mouse position
        Vector2 direction = mousePosition - transform.position;

        // Set the bullet's velocity to move in the direction of the mouse position
        rb.velocity = direction.normalized * speed;
        Debug.Log(rb.velocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        var enemy = collision.GetComponent<DamagableEntity>();
        if (enemy != null)
        {
            if (collision.tag == "BossWeakPoint")
            {
                collision.GetComponent<BossWeakPointScript>().TakeDamage(damage, transform);
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
            else if (collision.tag != "Player")
            {
                Debug.Log("TONS OF DMG");
                enemy.TakeDamage(damage);
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }    
        }
       
    }
}
