using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyParticles : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 3f);
    }
}
