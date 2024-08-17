using UnityEngine;

public class WeakPointScript : MonoBehaviour
{
    [SerializeField] EnemyScript enemyScript;


    public void takeDamage(int damage)
    {
        invokeScale();
    }

    private void invokeScale() { 
     
        if (enemyScript != null)
        {
            enemyScript.TriggerScaling(false);
        }
    
    }
}
