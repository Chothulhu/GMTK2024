using UnityEngine;

public class StrongPointScript : MonoBehaviour, DamagableEntity
{
    [SerializeField] EnemyScript enemyScript;

    public void TakeDamage(int damage)
    {
        InvokeScale();
    }

    private void InvokeScale()
    {

        if (enemyScript != null)
        {
            enemyScript.TriggerScaling(true);
        }

    }
}
