using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammopack : MonoBehaviour
{
    [SerializeField] private int ammo = 10;

    public int Ammo
    {
        get => ammo;
        set
        {
            ammo = value;
        }
    }
}
