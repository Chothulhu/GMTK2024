using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakPointScript : MonoBehaviour, DamagableEntity
{
    [SerializeField] BossScript bossScript;

    public void TakeDamage(int damage)
    {

    }

    public void TakeDamage(int damage, Transform position)
    {
        InvokeScale(position);
    }

    private void InvokeScale(Transform position)
    {
        if (bossScript != null)
        {
            bossScript.TriggerScaling(false, position);
        }
    }
}
