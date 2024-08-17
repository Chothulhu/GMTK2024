using System;
using UnityEngine;

public class WeakPointScript : MonoBehaviour, DamagableEntity
{
    [SerializeField] EnemyScript enemyScript;

    public void TakeDamage(int damage)
    {
        InvokeScale();
        
    }

    private void InvokeScale() {

        if (enemyScript != null)
        {
            enemyScript.TriggerScaling(false);
        }

    }
}
