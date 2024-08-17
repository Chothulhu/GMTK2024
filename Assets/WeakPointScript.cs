using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPointScript : MonoBehaviour
{
    [SerializeField] EnemyScript enemyScript;

    // Update is called once per frame
    void Update()
    {
        //if hit 
        invokeScale();
    }
    
    private void invokeScale()
    {

        if (enemyScript != null)
        {
            enemyScript.TriggerScaling(false);
        }

    }
}
