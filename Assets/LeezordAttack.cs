using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeezordAttack : MonoBehaviour
{
    private EnemyScript enemyScript;
    private Animator anim;
    private GlobalsScript globalsScript;
    private Transform target;
    [SerializeField] private float radius;
    [SerializeField] private float damage;

    private void Awake()
    {
        enemyScript = GetComponent<EnemyScript>();
        globalsScript = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GlobalsScript>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        target = globalsScript.playerPosition;
    }

    private void Update()
    {
        float distance = Vector2.Distance(target.position, transform.position);
        if (Mathf.Abs(distance) < radius)
        {
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
    }
}
