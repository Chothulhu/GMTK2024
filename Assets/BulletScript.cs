using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public int damage = 40;
    // Start is called before the first frame update
    void Start()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the bullet to the mouse position
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Set the bullet's velocity to move in the direction of the mouse position
        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //zameni za enemy kad bude imao nemoj covek sam sebe da puca
        var enemy = collision.GetComponent<PlayerScript>();
        if (enemy != null)
        {
            enemy.takeDamage(damage);
             Destroy(gameObject);
        }
       
    }
}
