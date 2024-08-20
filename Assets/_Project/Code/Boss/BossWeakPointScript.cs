using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakPointScript : MonoBehaviour, DamagableEntity
{
    [SerializeField] BossScript bossScript;

    public void TakeDamage(int damage)
    {
        InvokeScale();
    }

    private void InvokeScale()
    {
        if (bossScript != null)
        {
            bossScript.TriggerScaling(false);
        }
    }
}
