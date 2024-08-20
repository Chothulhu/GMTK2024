using UnityEngine;

public class EnemyCombatScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] LayerMask layerToDamage;
    [SerializeField] private int damage;
    [SerializeField] private float attackAnimationDelay;
    private bool inAnimation = false;

    private void OnEnable()
    {
        inAnimation = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var entity = collision.GetComponent<DamagableEntity>();
        if (entity != null)
        {
            Debug.Log("Te Aknem poziv!");
            // Animator
            if (animator != null && !inAnimation)
            {
                inAnimation = true;
                animator.SetTrigger("Attack");
            }
                
        }
    }

    void Attack() {
        // Detect damagable entities
        Collider2D[] hitEntities = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, layerToDamage);

        foreach (Collider2D entity in hitEntities) {
            Debug.Log("Te Aknem!" + entity.name);
            entity.GetComponent<DamagableEntity>().TakeDamage(damage);
        }
        
    }

    void EndAnimation()
    {
        inAnimation = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawSphere(attackPoint.position, attackRange);
    }
}
