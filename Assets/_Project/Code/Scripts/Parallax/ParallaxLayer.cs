using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private float parallaxMultiplier;

    public void Move(float delta)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.x -= delta * parallaxMultiplier;

        transform.localPosition = newPosition;
    }
}
