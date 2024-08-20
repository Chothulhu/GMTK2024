using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallHedge;

public class LeezordAttack : MonoBehaviour
{
    private EnemyScript enemyScript;
    private Animator anim;
    private GlobalsScript globalsScript;
    private Transform target;
    [SerializeField] private float radiusDetect;
    [SerializeField] private int damage;
    private bool inAnimation = false;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] LayerMask layerToDamage;

    private void Awake()
    {
        enemyScript = GetComponent<EnemyScript>();
        globalsScript = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GlobalsScript>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        inAnimation = false;
    }

    private void Start()
    {
        target = globalsScript.playerPosition;
    }

    private void Update()
    {
        float distance = Vector2.Distance(target.position, transform.position);
        if (Mathf.Abs(distance) < radiusDetect && !inAnimation)
        {
            inAnimation = true;
            StartAttack();
        }
    }

    private void StartAttack()
    {
        enemyScript.speed = 0f;
        // trigger animation and place event at the and that calls Explode 
        anim.SetTrigger("isAttack");
    }

    public void LeezordExplode()
    {
        Debug.Log("Boom");
        SmallHedge.SoundManager.PlaySound(SoundType.EXPLOSION, null, 0.6f);
        anim.SetTrigger("Explode");
        // Detect damagable entities
        Collider2D[] hitEntities = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, layerToDamage);

        foreach (Collider2D entity in hitEntities)
        {
            Debug.Log("Te Aknem!" + entity.name);
            entity.GetComponent<DamagableEntity>().TakeDamage(damage);
        }
        inAnimation = false; 
    }

    public void RemoveLeezrd()
    {
        enemyScript.Die();
    }
}
