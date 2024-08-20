using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropScript : MonoBehaviour
{
    private Transform bossPosition;
    [SerializeField] private float speed;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bossPosition = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GlobalsScript>().bossPosition;
    }

    private void OnEnable()
    {
        rb.gravityScale = 1;
    }

    public void GoToBoss()
    {
        Debug.Log("Collect me");
        rb.excludeLayers -= LayerMask.GetMask("Boss");
        Vector2 direction = bossPosition.position - transform.position;
        rb.gravityScale = 0;
        rb.velocity = direction.normalized * speed;
    }
}
