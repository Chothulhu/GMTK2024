using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPointScript : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;
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
     
        if (playerScript != null)
        {
            playerScript.TriggerScaling(false);
        }
    
    }
}
