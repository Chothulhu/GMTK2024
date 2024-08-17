using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DamagableEntity
{
    // Method to apply damage to the entity
    void TakeDamage(int amount);
}