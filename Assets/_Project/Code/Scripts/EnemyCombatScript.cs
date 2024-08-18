using UnityEngine;

public class EnemyCombatScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] LayerMask layerToDamage;
    [SerializeField] private int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        var entity = collision.GetComponent<DamagableEntity>();
        if (entity != null)
        {
            Debug.Log("Te Aknem poziv!");
            // Delay for animation
            Attack();
        }

    }

    void Attack() {

        

        // Animator
        if (animator != null)
        animator.SetTrigger("Attack");

        // Detect damagable entities
        Collider2D[] hitEntities = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, layerToDamage);

        foreach (Collider2D entity in hitEntities) {
            Debug.Log("Te Aknem!" + entity.name);
            entity.GetComponent<DamagableEntity>().TakeDamage(damage);
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawSphere(attackPoint.position, attackRange);
    }
}
