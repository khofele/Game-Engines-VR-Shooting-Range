using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static int points = 0;
    private static int ammo = 100;

    public static int Points
    {
        get => points;
    }

    public static int Ammo
    {
        get => ammo;
        set
        {
            ammo = value;
        }
    }
}
