 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float health = 60f;

    public void TakeDamage(float damage)
    {
        if(health > 0)
        {
            health -= damage;
            // DEBUG DIE
        }
    }
}
