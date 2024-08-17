using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPointScript : MonoBehaviour
{
    [SerializeField] EnemyScript enemyScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if hit 
        invokeScale();
    }

    private void invokeScale() { 
     
        if (enemyScript != null)
        {
            enemyScript.TriggerScaling(false);
        }
    
    }
}
