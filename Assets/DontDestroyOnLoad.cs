using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static DontDestroyOnLoad instance; 

        void Awake()
        {
            if(instance == null) instance = this;
            else if (instance != this) Destroy(this.gameObject);

            DontDestroyOnLoad(this);
        }
}
